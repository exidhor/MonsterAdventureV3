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

        public void InstanciateTrace(TracedObject tracedObject)
        {
            if (tracedObject.IsInstanciated)
            {
                Debug.Log("Try to instanciate " + tracedObject + " but the object is already instanciated.");
            }

            GameObject newGameObject = GetPoolAllocator(tracedObject.Trace).GetFreeResource();
            tracedObject.Instanciate(newGameObject);
        }

        public void ReleaseGameObject(TracedObject tracedObject)
        {
            if (!tracedObject.IsInstanciated)
            {
                Debug.Log("Try to release " + tracedObject + " but the object is already released.");
            }

            GetPoolAllocator(tracedObject.Trace).ReleaseResource(tracedObject.GameObject);
            tracedObject.Release();
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
