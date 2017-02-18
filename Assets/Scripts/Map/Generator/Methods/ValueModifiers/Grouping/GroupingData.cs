using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.Generation
{
    public class GroupingData : ValueModifierData
    {
        public GenerationData dataSource;
        public float StartOffset = 0;
        public List<GroupingValue> groupingValues;

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
            return new Grouping(this, generationTable);
        }

        public override GenerationData GetSource()
        {
            return dataSource;
        }
    }
}