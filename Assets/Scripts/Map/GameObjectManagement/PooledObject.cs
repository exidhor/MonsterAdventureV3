using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    [Serializable]
    public class PooledObject
    {
        public GameObject GameObject;
        public int PoolInstanceId;

        public override string ToString()
        {
            return GameObject.name + " ( id : " + PoolInstanceId + " )";
        }
    }
}
