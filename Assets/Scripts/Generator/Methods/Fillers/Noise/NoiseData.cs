using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.Generation
{
    public class NoiseData : FillerData
    {
        public NoiseGenerator noiseGenerator;

        public RandomGenerator randomGenerator
        {
            get { return _randomGenerator; }
        }

        public Transform noiseTransform
        {
            get { return _map.transform; }
        }

        protected RandomGenerator _randomGenerator;
        protected Map _map;

        protected override void AwakeContent()
        {
            _map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();
            _randomGenerator = _map.randomGenerator;
        }

        protected override GenerationMethod ConstructGenerationMethod(GenerationTable generationTable)
        {
            return new NoiseSpread(this, generationTable);
        }
    }
}