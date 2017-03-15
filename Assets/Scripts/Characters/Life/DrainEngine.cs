using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;
using UnityEngine;

namespace MonsterAdventure
{
    public class DrainEngine : MonoSingleton<DrainEngine>
    {
        public SoulBubble Model;
        public int PoolSize = 100;
        public int ExpandSize = 10;

        private Pool _soulBubblePool;
        private PoolRequest _poolRequestBuffer;

        private List<PoolObject> _toRelease;

        void Awake()
        {
            _soulBubblePool = PoolTable.Instance.AddPool(Model.gameObject, (uint)PoolSize, false, (uint)ExpandSize);
            _poolRequestBuffer = new PoolRequest();

            _toRelease = new List<PoolObject>();
        }

        public void ConstructSoulBubble(Vector2 start, DrainComponent target, float speed, float life)
        {
            _poolRequestBuffer.InitAllocation(_soulBubblePool, 1);

            PoolAllocator.Instance.ResolveInstantPoolRequest(_poolRequestBuffer);

            SoulBubble soulBubble = _poolRequestBuffer.PoolObjects[0].GameObject.GetComponent<SoulBubble>();
            soulBubble.Init(start, target, speed, life, _poolRequestBuffer.PoolObjects[0]);
        }

        public void ReleaseSoulBubble(SoulBubble soulBubble)
        {
            _toRelease.Add(soulBubble.GetPoolObject());
        }


        void Update()
        {
            if (_toRelease.Count > 0)
            {
                if (_toRelease.Count > 100)
                {
                    Debug.Log("Big number of bubble to release : " + _toRelease.Count);
                }

                _poolRequestBuffer.InitRelease(_toRelease);

                PoolAllocator.Instance.ResolveInstantPoolRequest(_poolRequestBuffer);

                _toRelease.Clear();
            }
        }
    }
}
