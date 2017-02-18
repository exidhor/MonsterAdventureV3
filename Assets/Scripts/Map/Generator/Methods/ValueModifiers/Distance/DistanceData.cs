using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.Generation
{
    public class DistanceData : ValueModifierData
    {
        public GroupingData dataSource;

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
            return new DistanceComputing(this, generationTable);
        }

        public override GenerationData GetSource()
        {
            return dataSource;
        }
    }
}
