using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.Generation
{
    [Serializable]
    public class InstancierValue
    {
        public int Value;
        public GameObject Prefab;
        public uint PoolSize;
        public bool IsStatic;
        public uint ExpandPoolSize;
    }
}
