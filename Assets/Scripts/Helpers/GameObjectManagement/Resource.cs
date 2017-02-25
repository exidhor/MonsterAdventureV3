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

        public Resource(GameObject gameObject)
        {
            this.GameObject = gameObject;
            IsUsed = false;
        }
    }
}
