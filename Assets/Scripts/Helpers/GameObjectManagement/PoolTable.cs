using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.Generation;
using UnityEngine;

namespace MonsterAdventure
{
    public class PoolTable : MonoSingleton<PoolTable>
    {
        private Dictionary<int, Pool> _table;

        private void Awake()
        {
            _table = new Dictionary<int, Pool>();
        }

        public Pool AddPool(GameObject model, uint poolSize, bool isStatic, uint expandSize = 1)
        {
            Pool newPool = InstanciatePool(model, isStatic, expandSize);
            newPool.SetSize(poolSize);

            _table.Add(model.GetInstanceID(), newPool);

            return newPool;
        }

        public Pool AddPool(PoolEntry poolEntry)
        {
            return AddPool(poolEntry.Prefab, 
                poolEntry.PoolSize, 
                poolEntry.IsStatic, 
                poolEntry.ExpandPoolSize);
        }

        public Pool GetPool(int instanceID)
        {
            return _table[instanceID];
        }

        private Pool InstanciatePool(GameObject model, bool isStatic, uint expandSize)
        {
            GameObject poolGameObject = new GameObject();
            poolGameObject.transform.parent = gameObject.transform;
            Pool pool = poolGameObject.AddComponent<Pool>();

            pool.transform.parent = gameObject.transform;

            pool.Construct(model, isStatic, expandSize);

            return pool;
        }

        public TracedObject GetTracedObject(InstancierValue instancierValue)
        {
            Pool pool = GetPool(instancierValue.Prefab.GetInstanceID());
            Trace trace = new Trace(instancierValue.Prefab.GetInstanceID());

            return new TracedObject(trace, pool);
        }
    }
}
