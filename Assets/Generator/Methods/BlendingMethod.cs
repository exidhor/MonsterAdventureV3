using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure
{
    public abstract class BlendingMethod : GenerationMethod
    {
        public delegate void BlendFunction(int x, int y, GenerationGrid destination, List<GenerationGrid> applications);

        private List<GenerationGrid> _applications;
        private BlendFunction _blendFunction;

        protected BlendingMethod(string name, uint level, GenerationTable generationTable, Type valueType, List<GenerationGrid> applications,
            BlendFunction blendFunction)
            : base(name, level, generationTable, valueType)
        {
            _applications = applications;
            _blendFunction = blendFunction;
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
