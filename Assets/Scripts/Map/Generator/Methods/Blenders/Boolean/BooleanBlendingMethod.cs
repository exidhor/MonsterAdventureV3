using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.Generation
{
    public class BooleanBlendingMethod : BlendingMethod
    {
        public BooleanBlendingMethod(BlendingData blendingData, GenerationTable generationTable)
            : base(blendingData, generationTable)
        {
            ComputeGeneration();
        }

        protected override void Blend(GenerationToken firstToken, GenerationToken secondToken, 
            GenerationToken result)
        {
            if (firstToken.GetBoolValue())
            {
                result.SetValue(GetBooleanBlendingData().valueToSetWhenBoolIsTrue);
            }
            else
            {
                result.SetValue(secondToken.GetFloatValue());
            }
        }

        private BooleanBlendingData GetBooleanBlendingData()
        {
            return (BooleanBlendingData) _generationData;
        }
    }
}