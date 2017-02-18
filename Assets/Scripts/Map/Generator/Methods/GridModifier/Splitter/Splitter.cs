using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.Generation
{
    public class Splitter : GridModifierMethod
    {
        private int _splitLineSize;

        public Splitter(SpliterData data, GenerationTable generationTable)
            : base(data, generationTable, GenerationType.Splitter, data.GetModifiedLevel())
        {
            ComputeSplitLineSize();
            ComputeGeneration();
        }

        protected override void FillGenerationGrid(GenerationGrid generationGrid)
        {
            GenerationGrid sourceGrid = GetSourceGrid();

            for (int x_source = 0; x_source < sourceGrid.lineSize; x_source++)
            {
                for (int y_source = 0; y_source < sourceGrid.lineSize; y_source++)
                {
                    FillSourceBox(x_source, y_source, sourceGrid, generationGrid);
                }
            }
        }

        private void ComputeSplitLineSize()
        {
            _splitLineSize = (int)Math.Pow(2, GetSplitterData().splitLevel);
        }

        private void FillSourceBox(int x_source, int y_source, 
            GenerationGrid sourceGrid,
            GenerationGrid targetGrid)
        {
            float sourceGridValue = sourceGrid.Get(x_source, y_source).GetFloatValue();

            int x_offset = x_source * _splitLineSize;
            int y_offset = y_source * _splitLineSize;

            for (int x_target = 0; x_target < _splitLineSize; x_target++)
            {
                for (int y_target = 0; y_target < _splitLineSize; y_target++)
                {
                    targetGrid.Get(x_target + x_offset, y_target + y_offset).SetValue(sourceGridValue);
                }
            }
        }

        private SpliterData GetSplitterData()
        {
            return (SpliterData) _generationData;
        }
    }
}