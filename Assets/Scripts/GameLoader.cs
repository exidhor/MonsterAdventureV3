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
        public RandomGenerator randomGenerator;

        void Start()
        {
            randomGenerator.Construct();

            map.Construct(randomGenerator);

            //map.Generate();
        }
    }
}
