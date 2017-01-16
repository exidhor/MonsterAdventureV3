using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure
{
    public class NoiseSpread : GenerationMethod
    {
        public NoiseSpread(string name, uint level, GenerationTable generationTable, Type valueType)
            : base(name, level, generationTable, valueType)
        {

        }

        protected override void FillGenerationGrid(GenerationGrid generationGrid)
        {
            throw new NotImplementedException();
        }
    }
}
