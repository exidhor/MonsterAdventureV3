using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    /// <summary>
    /// Store the Data necessary to create a Pool
    /// into the PoolTable 
    /// </summary>
    [Serializable]
    public class PoolEntry
    {
        public GameObject Prefab = null;
        public uint PoolSize = 100;
        public bool IsStatic = false;
        public uint ExpandPoolSize = 10;
    }
}
