using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class PoolObject : PoolRegister
    {
        public GameObject GameObject;

        public PoolObject(int poolId)
        {
            Pool = PoolTable.Instance.GetPool(poolId);

            if (Pool == null)
            {
                Debug.LogError("Impossible to find the pool at the id : " + poolId
                    + " in the object " + this);
            }

            Init();
        }

        public PoolObject(Pool pool)
        {
            Pool = pool;

            Init();
        }

        private void Init()
        {
            GameObject = null;
            IndexInPool = -1;
        }

        public virtual void Instantiate()
        {
            Pool.GetFreeResource(this);
        }

        public virtual void Release()
        {
            Pool.ReleaseResource(this);
        }

        public override string ToString()
        {
            return GameObject.name + " ( pool : " + Pool.name + " )";
        }
    }
}