using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    [Serializable]
    public class Trace
    {
        public int InstanceID;
        public Vector2 Position;

        public Trace(int instanceID)
        {
            InstanceID = instanceID;
        }
    }
}
