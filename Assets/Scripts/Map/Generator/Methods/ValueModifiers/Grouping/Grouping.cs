﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.Generation
{
    public class Grouping : ValueModifierMethod
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
                    float oldValue = GetGroupingData().GetSource().GetGenerationMethod().GetToken(x, y).GetFloatValue();

                    int newValue = FindNewValueValue(oldValue);

                    generationGrid.Get(x, y).SetValue(newValue);
                }
            }
        }

        private int FindNewValueValue(float oldvalue)
        {
            float actualSample = GetGroupingData().StartOffset;

            if (oldvalue < actualSample)
            {
                return -1;
            }

            for (int i = 0; i < GetGroupingData().groupingValues.Count; i++)
            {
                actualSample += GetGroupingData().groupingValues[i].offset;

                if (actualSample >= oldvalue)
                    return i;
            }

            return -1;
        }

        private GroupingData GetGroupingData()
        {
            return (GroupingData) _generationData;
        }

        public Color GetColor(int index)
        {
            if (index < 0 || index >= GetGroupingData().groupingValues.Count)
            {
                //Debug.Log("index : " + index);

                // return the default color if the value is not groupped
                return new Color(); 
            }

            return GetGroupingData().groupingValues[index].color;
        }
    }
}