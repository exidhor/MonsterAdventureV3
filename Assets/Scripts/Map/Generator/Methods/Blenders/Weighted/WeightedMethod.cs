using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Debug = UnityEngine.Debug;

namespace MonsterAdventure.Generation
{
    public class WeightedMethod : BlendingMethod
    {
        public WeightedMethod(BlendingData data, GenerationTable generationTable)
            : base(data, generationTable)
        {
            ComputeGeneration();
        }

        protected override void Blend(GenerationToken firstToken, GenerationToken secondToken,
            GenerationToken result)
        {
            float firstCoef = GetWeightedData().coefForFirstSource;
            float firstValue = firstToken.GetFloatValue();

            float secondCoef = GetWeightedData().coefForSecondSource;
            float secondValue = secondToken.GetFloatValue();

            float firstResult = firstCoef * firstValue;
            float secondResult = secondCoef * secondValue;

            switch (GetWeightedData().blendSign)
            {
                case BlendSign.Add:
                    result.SetValue(firstResult + secondResult);
                    break;

                case BlendSign.Soustract:
                    result.SetValue(firstResult - secondResult);
                    break;

                case BlendSign.Multiply:
                    result.SetValue(firstResult * secondResult);
                    break;

                case BlendSign.Divid:
                    result.SetValue(firstResult / secondResult);
                    break;

                default:
                    Debug.Log("unknown blending sign");
                    break;
            }
        }

        private WeightedData GetWeightedData()
        {
            return (WeightedData) _generationData;
        }
    }
}
