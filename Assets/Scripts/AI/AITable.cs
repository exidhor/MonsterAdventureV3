using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    public class AITable : MonoBehaviour
    {
        // -------------- SINGLETON PART -------------------------

        private static AITable _instance = null;

        public static AITable Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("AI Table");
                    _instance = go.AddComponent<AITable>();
                }

                return _instance;
            }
        }

        public static AITable NotModifiedInstance
        {
            get
            {
                return _instance;
            }
        }

        // ------------------------------------------------------

        private Dictionary<int, AIComponent> _table;


        private void Awake()
        {
            _table = new Dictionary<int, AIComponent>();
        }

        private void OnDestroy()
        {
            _instance = null;
        }

        public void Register(AIComponent component)
        {
            _table.Add(component.gameObject.GetInstanceID(), component);
        }

        public void Unregister(int instanceID)
        {
            _table.Remove(instanceID);
        }

        public AIComponent Get(int instanceID)
        {
            return _table[instanceID];
        }
    }
}
