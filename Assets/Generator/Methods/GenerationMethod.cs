using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure
{
    public abstract class GenerationMethod
    {
        private string _name;
        private GenerationGrid _generationGrid;
        private int _id;

        protected abstract void FillGenerationGrid(GenerationGrid generationGrid);

        public GenerationMethod(string name, 
                                uint level, 
                                GenerationTable generationTable, 
                                Type valueType)
        {
            _name = name;

            _id = generationTable.Register(this);

            _generationGrid = new GenerationGrid(level, _id, valueType);
        }

        public void ComputeGeneration()
        {
            FillGenerationGrid(_generationGrid);
        }

        public GenerationGrid GetGrid()
        {
            return _generationGrid;
        }
    }
}