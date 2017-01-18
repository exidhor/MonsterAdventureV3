using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.Generation
{
    public class Grouping : ModifierMethod
    {
        public Grouping(GroupingData data, GenerationTable generationTable)
            : base(data, generationTable, GenerationType.Grouping)
        {
            ComputeGeneration();
        }

        protected override void FillGenerationGrid(GenerationGrid generationGrid)
        {
            for (int y = 0; y < generationGrid.lineSize; y++)
            {
                for (int x = 0; x < generationGrid.lineSize; x++)
                {
                    float oldValue = GetgroupingData().source.GetGenerationMethod().GetToken(x, y).GetFloatValue();

                    int newValue = FindNewValueValue(oldValue);

                    generationGrid.Get(x, y).SetValue(newValue);
                }
            }
        }

        private int FindNewValueValue(float oldvalue)
        {
            float actualSample = 0;

            for (int i = 0; i < GetgroupingData().groupingValues.Count; i++)
            {
                actualSample += GetgroupingData().groupingValues[i].offset;

                if (actualSample >= oldvalue)
                    return i;
            }

            // impossible to reach normaly
            return -1;
        }

        private GroupingData GetgroupingData()
        {
            return (GroupingData) _generationData;
        }

        public Color GetColor(int index)
        {
            return GetgroupingData().groupingValues[index].color;
        }
    }
}