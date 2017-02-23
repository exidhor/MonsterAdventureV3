using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    public class SteeringTable : MonoBehaviour
    {
        private static SteeringTable _instance = null;

        public static SteeringTable Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("Steering Table");
                    _instance = go.AddComponent<SteeringTable>();
                }

                return _instance;
            }
        }

        public static SteeringTable NotModifiedInstance
        {
            get
            {
                return _instance;
            }
        }

        public List<SteeringEntry> SteeringEntries;

        private Dictionary<EBehavior, Pool> _table;

        private PoolRequest _poolRequestBuffer;

        private void Awake()
        {
            // we register this instance if it is created in the editor
            _instance = this;

            _table = new Dictionary<EBehavior, Pool>();
        }

        private void OnDestroy()
        {
            _instance = null;
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

            Pool pool = _table[behavior];

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
    }
}
