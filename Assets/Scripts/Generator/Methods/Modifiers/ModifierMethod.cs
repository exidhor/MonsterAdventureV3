using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.Generation
{
    public abstract class ModifierMethod : GenerationMethod
    {
        public ModifierMethod(GenerationData generationData,
            GenerationTable generationTable,
            GenerationType genererationType)
            : base(generationData, generationTable, genererationType)
        {
            // nothing ? 
        }
    }
}
