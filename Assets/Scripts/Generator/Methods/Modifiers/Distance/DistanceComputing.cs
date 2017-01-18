using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.Generation
{
    public class DistanceComputing : ModifierMethod
    {
        public DistanceComputing(GenerationData generationData, GenerationTable generationTable, GenerationType genererationType)
            : base(generationData, generationTable, genererationType)
        {

        }

        protected override void FillGenerationGrid(GenerationGrid generationGrid)
        {
            throw new NotImplementedException();
        }
    }
}
