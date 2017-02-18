using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.Generation
{
    public abstract class ValueModifierMethod : GenerationMethod
    {
        public ValueModifierMethod(ValueModifierData data,
            GenerationTable generationTable,
            GenerationType genererationType)
            : base(data, generationTable, genererationType, data.GetSource().GetLevel())
        {
            // nothing ? 
        }

        protected GenerationData GetSourceData()
        {
            return GetModifierData().GetSource();
        }

        protected GenerationGrid GetSourceGrid()
        {
            return GetSourceData().GetGenerationMethod().GetGrid();
        }

        private ValueModifierData GetModifierData()
        {
            return (ValueModifierData) _generationData;
        }
    }
}