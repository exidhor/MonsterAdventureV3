using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.Generation
{
    public abstract class GridModifierMethod : GenerationMethod
    {
        public GridModifierMethod(GridModifierData data,
            GenerationTable generationTable,
            GenerationType generationType,
            uint level)
            : base(data, generationTable, generationType, level) 
        {
            // nothing
        }

        protected GenerationData GetSourceData()
        {
            return GetModifierData().GetSource();
        }

        protected GenerationGrid GetSourceGrid()
        {
            return GetSourceData().GetGenerationMethod().GetGrid();
        }

        private GridModifierData GetModifierData()
        {
            return (GridModifierData) _generationData;
        }
    }
}
