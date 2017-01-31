using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class PoolAllocatorTable : MonoBehaviour
    {
        private PoolAllocator Prefab;

        private Dictionary<int, PoolAllocator> _table;

        private void Awake()
        {
            _table = new Dictionary<int, PoolAllocator>();
        }

        public void AddPoolAllocator(GameObject model, bool isStatic, uint expandSize = 1)
        {
            _table.Add(model.GetInstanceID(), InstanciatePoolAllocator(model, isStatic, expandSize));
        }

        public PoolAllocator GetPoolAllocator(int instanceID)
        {
            return _table[instanceID];
        }

        public PoolAllocator GetPoolAllocator(Trace trace)
        {
            return GetPoolAllocator(trace.InstanceID);
        }

        public GameObject InstanciateTrace(Trace trace)
        {
            return GetPoolAllocator(trace).GetFreeResource();
        }

        private PoolAllocator InstanciatePoolAllocator(GameObject model, bool isStatic, uint expandSize)
        {
            PoolAllocator poolAllocator = Instantiate<PoolAllocator>(Prefab);
            poolAllocator.transform.parent = gameObject.transform;

            poolAllocator.Construct(model, isStatic, expandSize);

            return poolAllocator;
        }
    }
}
