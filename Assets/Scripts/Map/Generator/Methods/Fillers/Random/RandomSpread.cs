using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.Generation
{
    public class RandomSpread : FillerMethod
    {
        public RandomSpread(RandomData data, GenerationTable generationTable)
            : base(data, generationTable, GenerationType.Random)
        {
            ComputeGeneration();
        }

        protected override void FillGenerationGrid(GenerationGrid generationGrid)
        {
            for (int y = 0; y < generationGrid.lineSize; y++)
            {
                for (int x = 0; x < generationGrid.lineSize; x++)
                {
                    float sample = RandomGenerator.Instance.NextFloat();

                    generationGrid.Get(x, y).SetValue(sample);
                }
            }
        }

        private RandomData GetRandomData()
        {
            return (RandomData) _generationData;
        }

        public Gradient GetDebugGradient()
        {
            return GetRandomData().debugGradient;
        }
    }
}
