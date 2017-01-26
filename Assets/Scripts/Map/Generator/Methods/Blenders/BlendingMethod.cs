using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.Generation
{
    public abstract class BlendingMethod : GenerationMethod
    {
        public delegate void BlendFunction(int x, int y, GenerationGrid destination, List<GenerationGrid> applications);

        private List<GenerationGrid> _applications;
        private BlendFunction _blendFunction;

        protected BlendingMethod(BlendingData blendingData, GenerationTable generationTable)
            : base(blendingData, generationTable, GenerationType.Blend, 0) // todo : change 0
        {
            //_applications = applications;
            //_blendFunction = blendFunction;

            ComputeGeneration();
        }

        protected abstract void Blend(GenerationGrid destination,
                      List<GenerationGrid> applications,
                      BlendFunction blendFunction);

        protected override void FillGenerationGrid(GenerationGrid generationGrid)
        {
            Blend(generationGrid, _applications, _blendFunction);
        }
    }
}
