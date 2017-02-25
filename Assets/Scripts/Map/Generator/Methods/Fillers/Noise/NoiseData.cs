using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.Generation
{
    [RequireComponent(typeof(NoiseGenerator))]
    public class NoiseData : FillerData
    {
        [HideInInspector]
        public NoiseGenerator noiseGenerator;

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
            noiseGenerator = GetComponent<NoiseGenerator>();
        }

        protected override GenerationMethod ConstructGenerationMethod(GenerationTable generationTable)
        {
            _map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();

            return new NoiseSpread(this, generationTable);
        }
    }
}