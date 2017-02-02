using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.Generation;
using UnityEngine;

namespace MonsterAdventure
{
    public class PoolAllocatorTable : MonoBehaviour
    {
        public PoolAllocator Prefab;

        private Dictionary<int, PoolAllocator> _table;

        private void Awake()
        {
            _table = new Dictionary<int, PoolAllocator>();
        }

        public void AddPoolAllocator(GameObject model, uint poolSize, bool isStatic, uint expandSize = 1)
        {
            PoolAllocator newPool = InstanciatePoolAllocator(model, isStatic, expandSize);
            newPool.SetSize(poolSize);

            _table.Add(model.GetInstanceID(), newPool);
        }

        public void AddPoolAllocator(InstancierValue instancierValue)
        {
            AddPoolAllocator(instancierValue.Prefab, 
                instancierValue.PoolSize, 
                instancierValue.IsStatic, 
                instancierValue.ExpandPoolSize);
        }

        public PoolAllocator GetPoolAllocator(int instanceID)
        {
            return _table[instanceID];
        }

        public PoolAllocator GetPoolAllocator(Trace trace)
        {
            return GetPoolAllocator(trace.InstanceID);
        }

        /*
        public void InstanciateTrace(TracedObject tracedObject)
        {
            if (tracedObject.IsInstanciated)
            {
                Debug.Log("Try to instanciate " + tracedObject + " but the object is already instanciated.");
            }

            PoolAllocator poolAllocator = GetPoolAllocator(tracedObject.Trace);

            tracedObject.Instanciate(poolAllocator);
        }

        public void ReleaseGameObject(TracedObject tracedObject)
        {
            if (!tracedObject.IsInstanciated)
            {
                Debug.Log("Try to release " + tracedObject + " but the object is already released.");
            }

            GetPoolAllocator(tracedObject.Trace).ReleaseResource(tracedObject.GameObject);
            tracedObject.Release();
        }*/

        private PoolAllocator InstanciatePoolAllocator(GameObject model, bool isStatic, uint expandSize)
        {
            PoolAllocator poolAllocator = Instantiate<PoolAllocator>(Prefab);
            poolAllocator.transform.parent = gameObject.transform;

            poolAllocator.Construct(model, isStatic, expandSize);

            return poolAllocator;
        }

        public TracedObject GetTracedObject(InstancierValue instancierValue)
        {
            PoolAllocator poolAllocator = GetPoolAllocator(instancierValue.Prefab.GetInstanceID());
            Trace trace = new Trace(instancierValue.Prefab.GetInstanceID());

            return new TracedObject(trace, poolAllocator);
        }
    }
}
