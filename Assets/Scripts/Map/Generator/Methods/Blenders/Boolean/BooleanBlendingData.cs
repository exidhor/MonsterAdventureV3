using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.Generation
{
    public class BooleanBlendingData : BlendingData
    {
        public float valueToSetWhenBoolIsTrue = 10f;

        public GenerationData dataApplication;

        public GenerationData dataApplicated;

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
            return new BooleanBlendingMethod(this, generationTable);
        }

        public override GenerationData GetFirstSource()
        {
            return dataApplication;
        }

        public override GenerationData GetSecondSource()
        {
            return dataApplicated;
        }
    }
}
