using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    [Serializable]
    public class Resource
    {
        public GameObject GameObject;
        public bool IsUsed;

        public Resource(GameObject GameObject)
        {
            this.GameObject = GameObject;
            IsUsed = false;
        }
    }
}
