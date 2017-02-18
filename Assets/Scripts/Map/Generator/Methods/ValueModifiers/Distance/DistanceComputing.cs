using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UnityEngine.UI;

namespace MonsterAdventure.Generation
{
    public class DistanceComputing : ValueModifierMethod
    {
        private List<Dictionary<int, List<Coords>>> _sortedDistancesPerValues;

        public DistanceComputing(DistanceData data, GenerationTable generationTable)
            : base(data, generationTable, GenerationType.Distance)
        {
            ComputeGeneration();
        }

        protected override void FillGenerationGrid(GenerationGrid generationGrid)
        {
            generationGrid.InitWith(-1);

            InitDistanceDictionary();

            List<List<Coords>> toSortPerGroups = ConstructToSortList(GetSourceGrid(), _sortedDistancesPerValues.Count);

            for (int groupIndex = 0; groupIndex < toSortPerGroups.Count; groupIndex++)
            {
                FindDistance(toSortPerGroups[groupIndex], _sortedDistancesPerValues[groupIndex], generationGrid);
            }
        }

        private void FindDistance(List<Coords> toSort, Dictionary<int, List<Coords>> sortedDistance,
            GenerationGrid destinationGrid)
        {
            // limit box (box adjacent to another grouping value)
            List<Coords> limitBox = FindLimitBox(toSort, destinationGrid);

            // then we check if no limit chunk were found
            if (limitBox.Count == 0)
            {
                // we add all the chunks at the max pos
                sortedDistance.Add(int.MaxValue, toSort);

                for (int i = 0; i < toSort.Count; i++)
                {
                    destinationGrid.Get(toSort[i]).SetValue(int.MaxValue);
                }

                // we stop the function
                return;
            }

            //else, we add the limitbox to the dictionary
            sortedDistance.Add(0, limitBox);

            int currentDistanceValue = 1;

            // then we iterate until all chunks are sorted
            while (toSort.Count > 0)
            {
                List<Coords> sortedCoordAtCurrentDistance = new List<Coords>();

                for (int i = 0; i < toSort.Count; i++)
                {
                    if (IsNearDeterminedDistance(toSort[i],
                        destinationGrid))
                    {
                        sortedCoordAtCurrentDistance.Add(toSort[i]);
                        toSort.RemoveAt(i);
                        i--;
                    }
                }

                if (sortedCoordAtCurrentDistance.Count > 0)
                {
                    // create an entry into the dictionary
                    sortedDistance.Add(currentDistanceValue, sortedCoordAtCurrentDistance);

                    // actualize distance (we cant do before because of the algo of "IsNearDeterminedDistance")
                    for (int i = 0; i < sortedCoordAtCurrentDistance.Count; i++)
                    {
                        destinationGrid.Get(sortedCoordAtCurrentDistance[i]).SetValue(currentDistanceValue);
                    }
                }

                // actualize the distance
                currentDistanceValue++;
            }
        }

        private void InitDistanceDictionary()
        {
            List<GroupingValue> groupingValues = GetGroupingSourceData().groupingValues;

            _sortedDistancesPerValues = new List<Dictionary<int, List<Coords>>>(groupingValues.Count);

            for (int i = 0; i < groupingValues.Count; i++)
            {
                _sortedDistancesPerValues.Add(new Dictionary<int, List<Coords>>());
            }
        }

        private List<List<Coords>> ConstructToSortList(GenerationGrid sourceGrid, int numberOfValues)
        {
            List<List<Coords>> toSortPerValues = new List<List<Coords>>(numberOfValues);

            for (int valueIndex = 0; valueIndex < numberOfValues; valueIndex++)
            {
                toSortPerValues.Add(new List<Coords>());
            }

            for (int i = 0; i < sourceGrid.lineSize; i++)
            {
                for (int j = 0; j < sourceGrid.lineSize; j++)
                {
                    int groupingValue = sourceGrid.Get(i, j).GetIntValue();

                    toSortPerValues[groupingValue].Add(new Coords(i, j));
                }
            }

            return toSortPerValues;
        }

        private List<Coords> FindLimitBox(List<Coords> toSort,
            GenerationGrid destinationGrid)
        {
            // limit box (box adjacent to another grouping value)
            List<Coords> limitBox = new List<Coords>();

            // at the first iteration, we find the limit
            for (int i = 0; i < toSort.Count; i++)
            {
                Coords currentCoords = toSort[i];

                if (IsTheLimit(currentCoords, GetSourceGrid()))
                {
                    limitBox.Add(currentCoords);
                    destinationGrid.Get(currentCoords).SetValue(0);
                    toSort.RemoveAt(i);
                    i--;
                }
            }

            return limitBox;
        }

        private GroupingData GetGroupingSourceData()
        {
            return (GroupingData) GetSourceData();
        }

        private DistanceData GetDistanceData()
        {
            return (DistanceData) _generationData;
        }

        private bool IsTheLimit(Coords coords, GenerationGrid grid)
        {
            if (grid.lineSize == 0)
                return false;

            int currentGroupingValue = grid.Get(coords).GetIntValue();

            int current_x = coords.abs;
            int current_y = coords.ord;

            // check at the left
            if (current_x > 0
                && grid.Get(current_x - 1, current_y).GetIntValue() != currentGroupingValue)
            {
                return true;
            }

            // check top
            if (current_y < grid.lineSize - 1
                && grid.Get(current_x, current_y + 1).GetIntValue() != currentGroupingValue)
            {
                return true;
            }

            // check right
            if (current_x < grid.lineSize - 1
                && grid.Get(current_x + 1, current_y).GetIntValue() != currentGroupingValue)
            {
                return true;
            }

            // check bot
            if (current_y > 0
                && grid.Get(current_x, current_y - 1).GetIntValue() != currentGroupingValue)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if the <see cref="Sector" /> at the specific position is near
        /// a distance determined <see cref="Sector" />.
        /// This function is usefull to organize the <see cref="Biome" />
        /// </summary>
        /// <param name="coords">The position in the chunk grid</param>
        /// <param name="allSectorsOfThisLevel">All the Sector of the <see cref="Biome" /></param>
        /// <returns><code>true</code> if the <see cref="Sector" /> is near a distance
        /// determined <see cref="Sector" />, <code>false</code> otherwise</returns>
        private bool IsNearDeterminedDistance(Coords coords, GenerationGrid destinationGrid)
        {
            // check at the left
            int currentDistance;

            if (coords.abs > 0)
            {
                currentDistance = destinationGrid.Get(coords.abs - 1, coords.ord).GetIntValue();

                if (currentDistance != -1)
                {
                    return true;
                }
            }

            // check top
            if (destinationGrid.lineSize > 0 && coords.ord < destinationGrid.lineSize - 1)
            {
                currentDistance = destinationGrid.Get(coords.abs, coords.ord + 1).GetIntValue();

                if (currentDistance != -1)
                {
                    return true;
                }
            }

            // check right
            if (coords.abs < destinationGrid.lineSize - 1)
            {
                currentDistance = destinationGrid.Get(coords.abs + 1, coords.ord).GetIntValue();

                if (currentDistance != -1)
                {
                    return true;
                }
            }

            // check bot
            if (coords.ord > 0)
            {
                currentDistance = destinationGrid.Get(coords.abs, coords.ord - 1).GetIntValue();

                if (currentDistance != -1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}