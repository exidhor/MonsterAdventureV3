using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace MonsterAdventure
{
    public enum PoolRequestAction
    {
        Free,
        Allocate
    }

    [Serializable]
    public class PoolRequest
    {
        public List<PoolObject> PoolObjects;
        public PoolRequestAction Action;

        public int Size
        {
            get
            {
                return PoolObjects.Count;
            }
        }

        public PoolRequest()
        {
            PoolObjects = new List<PoolObject>();
        }

        public PoolRequest(List<PoolObject> poolObjects, PoolRequestAction action)
        {
            Init(poolObjects, action);
        }

        public PoolRequest(PoolObject poolObject, PoolRequestAction action)
            : this()
        {
            PoolObjects.Add(poolObject);

            Action = action;
        }

        public PoolRequest(Pool pool, int count)
            : this()
        {
            InitAllocation(pool, count);
        }

        public PoolRequest(int poolId, int count)
                : this(PoolTable.Instance.GetPool(poolId), count)
        {
            // nothing    
        }

        public void Init(List<PoolObject> poolObjects, PoolRequestAction action)
        {
            PoolObjects = poolObjects;
            Action = action;
        }

        public void InitAllocation(Pool pool, int count)
        {
            Action = PoolRequestAction.Allocate;

            ResetPoolList(count);

            for (int i = 0; i < count; i++)
            {
                PoolObjects.Add(new PoolObject(pool));
            }
        }

        public void InitRelease(PoolObject poolObject)
        {
            Action = PoolRequestAction.Free;
           
            ResetPoolList(1);

            PoolObjects.Add(poolObject);
        }

        public override string ToString()
        {
            return "PoolRequest (Count : " + PoolObjects.Count + ", Action : " + Action + ")";
        }

        private void ResetPoolList(int capacity)
        {
            if (capacity != PoolObjects.Capacity)
            {
                PoolObjects.Capacity = capacity;
            }
            PoolObjects.Clear();
        }
    }
}
