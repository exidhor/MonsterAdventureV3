using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.Generation
{
    public abstract class GenerationMethod
    {
        protected GenerationData _generationData;

        private GenerationGrid _generationGrid;
        private int _id;
        private GenerationType _generationType;

        protected abstract void FillGenerationGrid(GenerationGrid generationGrid);

        public GenerationMethod(GenerationData generationData, 
                                GenerationTable generationTable, 
                                GenerationType genererationType,
                                uint level)
        {
            _generationData = generationData;

            _id = generationTable.Register(this);

            _generationGrid = new GenerationGrid(level, _id);

            _generationType = genererationType;
        }

        public void ComputeGeneration()
        {
            FillGenerationGrid(_generationGrid);
        }

        public GenerationGrid GetGrid()
        {
            return _generationGrid;
        }

        public string GetName()
        {
            return _generationData.name;
        }

        public uint GetLevel()
        {
            return _generationGrid.GetLevel();
        }

        public GenerationToken GetToken(int x, int y)
        {
            return _generationGrid.Get(x, y);
        }

        public GenerationType GetGenerationType()
        {
            return _generationType;
        }
    }
}