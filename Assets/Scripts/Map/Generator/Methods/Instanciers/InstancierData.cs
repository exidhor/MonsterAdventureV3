using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.Generation
{
    public class InstancierData : GenerationData
    {
        public GroupingData dataSource;

        public List<InstancierValue> InstancierValues;

        protected override void AwakeContent()
        {
            // nothing
        }

        protected override GenerationMethod ConstructGenerationMethod(GenerationTable generationTable)
        {
            return new InstancierMethod(this, generationTable);
        }
    }
}
