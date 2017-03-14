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

        void Awake()
        {
            _soulBubblePool = PoolTable.Instance.AddPool(Model.gameObject, (uint)PoolSize, false, (uint)ExpandSize);
            _poolRequestBuffer = new PoolRequest();
        }

        public void ConstructSoulBubble(Vector2 start, Location destination, float speed, float life)
        {
            _poolRequestBuffer.InitAllocation(_soulBubblePool, 1);

            PoolAllocator.Instance.ResolveInstantPoolRequest(_poolRequestBuffer);

            SoulBubble soulBubble = _poolRequestBuffer.PoolObjects[0].GameObject.GetComponent<SoulBubble>();
            soulBubble.Init(start, destination, speed, life, _poolRequestBuffer.PoolObjects[0]);
        }

        public void ConstructSoulBubble(Vector2 start, Transform destination, float speed, float life)
        {
            ConstructSoulBubble(start, new TransformLocation(destination), speed, life);
        }
    }
}
