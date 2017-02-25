using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    public class SteeringTable : MonoSingleton<SteeringTable>
    {
        public List<SteeringEntry> SteeringEntries;

        public int TableCount
        {
            get { return _table.Count; }
        }

        //private Dictionary<EBehavior, Pool> _table;
        private MappedList<EBehavior, Pool> _table;

        private PoolRequest _poolRequestBuffer;

        private void Awake()
        {
            //_table = new Dictionary<EBehavior, Pool>();
            _table = new MappedList<EBehavior, Pool>();
        }

        private void Start()
        {
            _poolRequestBuffer = new PoolRequest();

            ConstructPools();
        }

        private void ConstructPools()
        {
            List<EBehavior> done = new List<EBehavior>();

            for (int i = 0; i < SteeringEntries.Count; i++)
            {
                bool isDone = false;

                for (int j = 0; j < done.Count; j++)
                {
                    if (SteeringEntries[i].Behavior == done[j])
                    {
                        Debug.LogWarning("Multiple times the same behavior entry " + done[j] + " !\n"
                                         + "This is not allowed, only the first will be keeped, "
                                         + SteeringEntries[i].Prefab + " will be discarded");

                        isDone = true;
                    }
                }

                if (!isDone)
                {
                    ConstructPool(SteeringEntries[i]);
                }
            }
        }

        private void ConstructPool(SteeringEntry steeringEntry)
        {
            Pool newPool = PoolTable.Instance.AddPool(steeringEntry);

            _table.Add(steeringEntry.Behavior, newPool);
        }

        public KinematicSteering GetFreeSteering(EBehavior behavior)
        {
            if (behavior == EBehavior.None)
                return null;

            Pool pool = _table.GetByKey(behavior);

            _poolRequestBuffer.InitAllocation(pool, 1);
            PoolAllocator.Instance.DoInstancePoolRequest(_poolRequestBuffer);

            KinematicSteering result = _poolRequestBuffer.PoolObjects[0].GameObject.GetComponent<KinematicSteering>();
            result.SetPoolObject(_poolRequestBuffer.PoolObjects[0]);

            return result;
        }

        public void ReleaseBusySteering(KinematicSteering steering)
        {
            _poolRequestBuffer.InitRelease(steering.GetPoolObject());

            PoolAllocator.Instance.DoInstancePoolRequest(_poolRequestBuffer);
        }

        // use it to iterate in the table
        public Pool GetPoolAt(int index)
        {
            return _table.GetByIndex(index);
        }
    }
}