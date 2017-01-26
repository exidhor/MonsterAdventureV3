using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.Generation
{
    public class NoiseSpread : FillerMethod
    {
        public NoiseSpread(NoiseData noiseData, GenerationTable generationTable)

            : this(noiseData, generationTable, GenerationType.Noise)
        {
            ComputeGeneration();
        }

        protected NoiseSpread(NoiseData noiseData, GenerationTable generationTable, GenerationType generationType)

            : base(noiseData, generationTable, generationType)
        {
            InitNoise(noiseData.level, noiseData.randomGenerator, noiseData.noiseTransform);
        }

        protected override void FillGenerationGrid(GenerationGrid generationGrid)
        {
            for (int y = 0; y < generationGrid.lineSize; y++)
            {
                for (int x = 0; x < generationGrid.lineSize; x++)
                {
                    float sample = GetNoiseData().noiseGenerator.Get(x, y);

                    generationGrid.Get(x, y).SetValue(sample);
                }
            }
        }

        private void InitNoise(uint level, RandomGenerator randomGenerator, Transform transform)
        {
            GetNoiseData().noiseGenerator.Construct();

            GetNoiseData().noiseGenerator.Generate((int)Math.Pow(2, level), transform, randomGenerator);
        }

        public Gradient GetDebugGradient()
        {
            return GetNoiseData().debugGradient;
        }

        private NoiseData GetNoiseData()
        {
            return (NoiseData) _generationData;
        }
    }
}
