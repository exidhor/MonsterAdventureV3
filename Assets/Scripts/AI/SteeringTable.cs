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

        private List<KinematicSeek> _seeks;

        private void Start()
        {
            //_seeks = 
        }
    }
}
