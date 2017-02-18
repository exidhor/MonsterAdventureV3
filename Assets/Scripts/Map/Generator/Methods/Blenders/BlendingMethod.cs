using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace MonsterAdventure.Generation
{
    public abstract class BlendingMethod : GenerationMethod
    {
        private bool _inverseGridOrder;

        protected BlendingMethod(BlendingData blendingData, GenerationTable generationTable)
            : base(blendingData, generationTable, GenerationType.Blend, blendingData.GetConstructionLevel())
        {
            // nothing
        }

        protected abstract void Blend(GenerationToken firstToken, GenerationToken secondToken, 
            GenerationToken result);

        protected override void FillGenerationGrid(GenerationGrid generationGrid)
        {
            GenerationGrid smallestGrid;
            GenerationGrid biggestGrid;

            SetGrids(out smallestGrid, out biggestGrid);

            GenerationToken bufferForSmallestGrid;
            GenerationToken bufferForBiggestGrid;

            //int secondIterationRange = biggestGrid.lineSize - smallestGrid.lineSize + 1;
            int offsetLevel = (int)(biggestGrid.GetLevel() - smallestGrid.GetLevel());
            int secondIterationRange = (int)Math.Pow(2, offsetLevel);

            for (int x_small = 0; x_small < smallestGrid.lineSize; x_small++)
            {
                for (int y_small = 0; y_small < smallestGrid.lineSize; y_small++)
                {
                    bufferForSmallestGrid = smallestGrid.Get(x_small, y_small);

                    for (int x_secondIteration = 0; x_secondIteration < secondIterationRange; x_secondIteration++)
                    {
                        for (int y_secondIteration = 0; y_secondIteration < secondIterationRange; y_secondIteration++)
                        {
                            int x_big = x_secondIteration + x_small*(secondIterationRange);
                            int y_big = y_secondIteration + y_small*(secondIterationRange);

                            bufferForBiggestGrid = biggestGrid.Get(x_big, y_big);

                            if (_inverseGridOrder)
                            {
                                Blend(bufferForBiggestGrid, bufferForSmallestGrid, generationGrid.Get(x_big, y_big));
                            }
                            else
                            {
                                Blend(bufferForSmallestGrid, bufferForBiggestGrid, generationGrid.Get(x_big, y_big));
                            }
                        }
                    }
                }
            }
        }

        private float GetFirstValue(int x, int y)
        {
            return GetBlendingData().GetFirstSource().GetGrid().Get(x, y).GetFloatValue();
        }

        private float GetSecondValue(int x, int y)
        {
            return GetBlendingData().GetSecondSource().GetGrid().Get(x, y).GetFloatValue();
        }

        private void SetGrids(out GenerationGrid smallestGrid, out GenerationGrid biggestGrid)
        {
            if (GetBlendingData().GetFirstSource().GetGrid().lineSize <
                GetBlendingData().GetSecondSource().GetGrid().lineSize)
            {
                smallestGrid = GetBlendingData().GetFirstSource().GetGrid();
                biggestGrid = GetBlendingData().GetSecondSource().GetGrid();

                _inverseGridOrder = false;
            }
            else
            {
                biggestGrid = GetBlendingData().GetFirstSource().GetGrid();
                smallestGrid = GetBlendingData().GetSecondSource().GetGrid();

                _inverseGridOrder = true;
            }
        }

        private BlendingData GetBlendingData()
        {
            return (BlendingData)_generationData;
        }
    }
}