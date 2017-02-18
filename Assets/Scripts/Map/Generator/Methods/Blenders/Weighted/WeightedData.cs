using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.Generation
{
    public enum BlendSign
    {
        Add,
        Soustract,
        Multiply,
        Divid
    }

    public class WeightedData : BlendingData
    {
        public GenerationData firstDataSource;
        public float coefForFirstSource;

        public BlendSign blendSign;

        public GenerationData secondDataSource;
        public float coefForSecondSource;

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
            return new WeightedMethod(this, generationTable);
        }

        public override GenerationData GetFirstSource()
        {
            return firstDataSource;
        }

        public override GenerationData GetSecondSource()
        {
            return secondDataSource;
        }
    }
}
