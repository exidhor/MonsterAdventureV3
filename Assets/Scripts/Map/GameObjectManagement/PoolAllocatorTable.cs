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
        public Pool Prefab;

        private Dictionary<int, Pool> _table;

        private void Awake()
        {
            _table = new Dictionary<int, Pool>();
        }

        public void AddPoolAllocator(GameObject model, uint poolSize, bool isStatic, uint expandSize = 1)
        {
            Pool newPool = InstanciatePoolAllocator(model, isStatic, expandSize);
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

        public Pool GetPoolAllocator(int instanceID)
        {
            return _table[instanceID];
        }

        public Pool GetPoolAllocator(Trace trace)
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

        private Pool InstanciatePoolAllocator(GameObject model, bool isStatic, uint expandSize)
        {
            Pool pool = Instantiate<Pool>(Prefab);
            pool.transform.parent = gameObject.transform;

            pool.Construct(model, isStatic, expandSize);

            return pool;
        }

        public TracedObject GetTracedObject(InstancierValue instancierValue)
        {
            Pool pool = GetPoolAllocator(instancierValue.Prefab.GetInstanceID());
            Trace trace = new Trace(instancierValue.Prefab.GetInstanceID());

            return new TracedObject(trace, pool);
        }
    }
}
