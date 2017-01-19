using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.Generation
{
    public class RandomData : FillerData
    {
        private RandomGenerator _randomGenerator;

        protected override void AwakeContent()
        {
            Map map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();
            _randomGenerator = map.randomGenerator;
        }

        protected override GenerationMethod ConstructGenerationMethod(GenerationTable generationTable)
        {
            return new RandomSpread(this, generationTable);
        }

        public RandomGenerator GetRandomGenerator()
        {
            return _randomGenerator;
        }
    }
}
