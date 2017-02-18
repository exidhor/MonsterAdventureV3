﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.Generation
{
    public class RandomData : FillerData
    {
        public RandomGenerator RandomGenerator
        {
            get { return _map.RandomGenerator; }
        }

        private Map _map;

        protected override void AwakeContent()
        {
            // nothing
        }

        protected override void StartContent()
        {

        }

        protected override GenerationMethod ConstructGenerationMethod(GenerationTable generationTable)
        {
            _map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();

            return new RandomSpread(this, generationTable);
        }
    }
}
