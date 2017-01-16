using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    /// <summary>
    /// Represents a specific ecostystem around the world.
    /// It means that a <see cref="Biome" /> define differents <see cref="Zone" /> 
    /// </summary>
    [RequireComponent(typeof(BiomeData))]
    public class Biome : MonoBehaviour
    {
        public List<Sector> sectors;

        [HideInInspector]
        public BiomeData data;
        private List<Dictionary<int, List<Sector>>> _sortedSectorsPerLevel;

        /// <summary>
        /// Initiliaze the parameters
        /// </summary>
        private void Awake()
        {
            sectors = new List<Sector>();

            data = GetComponent<BiomeData>();
        }

        /// <summary>
        /// Extend the <see cref="Biome" /> with the <see cref="Sector" />
        /// </summary>
        /// <param name="sector">The new Sector</param>
        public void Add(Sector sector)
        {
            sectors.Add(sector);
        }

        /// <summary>
        /// Retrieve every <see cref="Sector" /> which are at the
        /// a minimal specific distance (in area unit) to an other <see cref="Biome" />
        /// </summary>
        /// <param name="minDistance">The minimal distance in area unit.</param>
        /// <returns>The <see cref="Sector" /> founded</returns>
        public List<Sector> GetSectorsFromMinDistance(int minDistance, int level)
        {
            List<Sector> foundedSectors = new List<Sector>();

            foreach (int distance in _sortedSectorsPerLevel[level].Keys)
            {
                if (distance >= minDistance)
                {
                    foundedSectors.AddRange(_sortedSectorsPerLevel[level][distance]);
                }
            }

            return foundedSectors;
        }

        /// <summary>
        /// Sort the <see cref="Sector" /> by distance to another <see cref="Biome" />
        /// </summary>
        /// <param name="allSectors">Every Chunks we want to sort</param>
        public void OrganizeLevel(FakeDoubleEntryList<Sector> allSectorsOfThisLevel, int resolution)
        {
            _sortedSectorsPerLevel = new List<Dictionary<int, List<Sector>>>(resolution + 1);

            for (int i = 0; i < resolution + 1; i++)
            {
                _sortedSectorsPerLevel.Add(new Dictionary<int, List<Sector>>());
            }

            List<Sector> toSort = new List<Sector>(sectors);

            // limit chunks (chunks near another biome)
            List<Sector> limitSectors = new List<Sector>();

            // at the first iteration, we set the chunk which are adjacent to
            // a chunk of another biome
            for (int i = 0; i < toSort.Count; i++)
            {
                if (Sector.BelongToBiomeLimit(toSort[i], allSectorsOfThisLevel))
                {
                    limitSectors.Add(toSort[i]);
                    toSort[i].SetDistanceToLimit(0);
                    toSort.RemoveAt(i);
                    i--;
                }
            }

            // then we check if no limit chunk were found
            if (limitSectors.Count == 0)
            {
                // we add all the chunks at the max pos
                _sortedSectorsPerLevel[resolution].Add(int.MaxValue, toSort);

                // we actualize the distance to limit in each chunk
                foreach (Sector chunk in toSort)
                {
                    chunk.SetDistanceToLimit(int.MaxValue);
                }

                // we stop the function
                return;
            }

            //else, we add the limit chunks to the dictionary
            _sortedSectorsPerLevel[resolution].Add(0, limitSectors);

            int currentDistanceToLimit = 1;

            // then we iterate until all chunks are sorted
            while (toSort.Count > 0)
            {
                List<Sector> chunksForCurrentDistance = new List<Sector>();

                for (int i = 0; i < toSort.Count; i++)
                {
                    if (IsNearDeterminedDistance(toSort[i].GetCoords(),
                                                 allSectorsOfThisLevel))
                    {
                        chunksForCurrentDistance.Add(toSort[i]);
                        toSort.RemoveAt(i);
                        i--;
                    }
                }

                if (chunksForCurrentDistance.Count > 0)
                {
                    // set the distance in the chunk
                    foreach (Sector chunk in chunksForCurrentDistance)
                    {
                        chunk.SetDistanceToLimit(currentDistanceToLimit);
                    }

                    // create an entry into the dictionary
                    _sortedSectorsPerLevel[resolution].Add(currentDistanceToLimit, chunksForCurrentDistance);
                }
                else
                {
                    Debug.LogError("No chunk found during an iteration !");
                }
                
                // actualize the distance
                currentDistanceToLimit++;
            }

            // debug infos to display dictionary content
            //Debug.Log("Dictionary - " + type + " (" + _sortedTiles.Count + ") ------------- ");
            //foreach (int distance in _sortedTiles.Keys)
            //{
            //    Debug.Log("Distance : " + distance + ", size : " +  _sortedTiles[distance].Count);
            //}
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
        private bool IsNearDeterminedDistance(Coords coords, FakeDoubleEntryList<Sector> allSectorsOfThisLevel)
        {
            // check at the left
            int currentDistance;

            if (coords.abs > 0)
            {
                currentDistance = allSectorsOfThisLevel.GetElement(coords.abs - 1, coords.ord).GetDistanceToLimit();

                if (currentDistance != -1)
                {
                    return true;
                }
            }

            // check top
            if (allSectorsOfThisLevel.lineSize > 0 && coords.ord < allSectorsOfThisLevel.lineSize - 1)
            {
                currentDistance = allSectorsOfThisLevel.GetElement(coords.abs, coords.ord + 1).GetDistanceToLimit();

                if (currentDistance != -1)
                {
                    return true;
                }
            }

            // check right
            if (coords.abs < allSectorsOfThisLevel.lineSize - 1)
            {
                currentDistance = allSectorsOfThisLevel.GetElement(coords.abs + 1, coords.ord).GetDistanceToLimit();

                if (currentDistance != -1)
                {
                    return true;
                }
            }

            // check bot
            if (coords.ord > 0)
            {
                currentDistance = allSectorsOfThisLevel.GetElement(coords.abs, coords.ord - 1).GetDistanceToLimit();

                if (currentDistance != -1)
                {
                    return true;
                }
            }

            return false;
        }

        public Dictionary<int, List<Sector>> GetSortedSectors(int level)
        {
            if (level >= _sortedSectorsPerLevel.Count)
                return null;

            return _sortedSectorsPerLevel[level];
        }

        public override string ToString()
        {
            return gameObject.name;
        }
    }
}