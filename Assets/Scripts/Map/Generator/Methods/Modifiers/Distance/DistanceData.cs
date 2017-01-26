using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.Generation
{
    public class DistanceData : ModifierData
    {
        public GroupingData dataSource;

        protected override void AwakeContent()
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
