using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class GameLoader : MonoBehaviour
    {
        public Map map;

        void Start()
        {
            map.Construct();

            map.Generate();
        }
    }
}
