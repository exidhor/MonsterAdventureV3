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
            get { return _map.RandomGenerator; }
        }

        public Transform noiseTransform
        {
            get { return _map.transform; }
        }

        protected Map _map;

        protected override void AwakeContent()
        {
            // nothing
        }

        protected override void StartContent()
        {
            // nothing
        }

        protected override GenerationMethod ConstructGenerationMethod(GenerationTable generationTable)
        {
            _map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();

            return new NoiseSpread(this, generationTable);
        }
    }
}