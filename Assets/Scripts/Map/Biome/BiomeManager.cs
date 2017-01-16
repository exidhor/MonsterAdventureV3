using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MonsterAdventure
{
    /// <summary>
    /// Construct and manage all the different <see cref="Biome" /> in the map.
    /// The <see cref="Biome" /> construction is done from the <see cref="NoiseGenerator" />.
    /// </summary>
    [RequireComponent(typeof(NoiseGenerator))]
    public class BiomeManager : MonoBehaviour
    {
        private NoiseGenerator _noiseGenerator;

        public Biome[] biomes;

        public uint targetLevel;

        private Dictionary<string, int> _biomeIndex;

        private bool _isInitiliazed;

        private void Awake()
        {
            _noiseGenerator = GetComponent<NoiseGenerator>();

            _isInitiliazed = false;
        }

        /// <summary>
        /// Construct the different <see cref="Biome" /> and construct
        /// the <see cref="NoiseGenerator" />
        /// </summary>
        public void Construct()
        {
            // normalize the ratio value
            float sum = 0;

            foreach (Biome biome in biomes)
            {
                sum += biome.data.ratio;
            }

            foreach (Biome biome in biomes)
            {
                biome.data.NormalizeRatio(sum);
            }

            // construct the dictionary
            _biomeIndex = new Dictionary<string, int>();

            for (int i = 0; i < biomes.Length; i++)
            {
                _biomeIndex.Add(biomes[i].gameObject.name, i);
            }

            _noiseGenerator.Construct();
        }

        /*
        /// <summary>
        /// Instantiate the <see cref="Biome" /> in the scene from the
        /// biome prefab.
        /// </summary>
        /// <param name="type">The BiomeType of the Biome</param>
        /// <returns>The new instantiated Biome</returns>
        private Biome InstantiateBiome(BiomeType type)
        {
            Biome biome = Instantiate<Biome>(biomePrefab);
            biome.transform.parent = gameObject.transform;
            biome.name = type.ToString();

            return biome;
        }*/

        /// <summary>
        /// Apply the noise value from the <see cref="NoiseGenerator" /> to the
        /// chunk grid to generate the different <see cref="Biome" />.
        /// </summary>
        /// <param name="sectorsPerLevel">The chunks list to fill</param>
        /// <param name="mapSize">The width/height of the map</param>
        /// <param name="random">The random generator</param>
        public void Generate(List<FakeDoubleEntryList<Sector>> sectorsPerLevel, RandomGenerator random)
        {
            int mapSize = (int)sectorsPerLevel[(int)targetLevel].lineSize;

            _noiseGenerator.Generate(mapSize, transform, random);

            ApplyNoise(sectorsPerLevel[(int)targetLevel]);

            SortPerBiomes(sectorsPerLevel[(int)targetLevel]);

            OrganizeBiomes(sectorsPerLevel[(int)targetLevel]);

            _isInitiliazed = true;
        }

        /// <summary>
        /// Blend the noise value with the <see cref="Chunk" /> to create <see cref="Biome" />
        /// </summary>
        /// <param name="sectors">The chunk grid</param>
        private void ApplyNoise(FakeDoubleEntryList<Sector> sectors)
        {
            for (int i = 0; i < sectors.lineSize; i++)
            {
                for (int j = 0; j < sectors.lineSize; j++)
                {
                    float sample = _noiseGenerator.Get(i, j);
                    Biome biome = FindBiomeWithSample(sample);

                    Sector sector = sectors.GetElement(i, j);
                    sector.AddBiome(biome, true, true);
                }
            }
        }

        /// <summary>
        /// Sort every <see cref="Chunk" /> per <see cref="BiomeType" />
        /// </summary>
        /// <param name="chunks">The chunk grid</param>
        private void SortPerBiomes(FakeDoubleEntryList<Sector> sectors)
        {
            for (int i = 0; i < sectors.lineSize; i++)
            {
                for (int j = 0; j < sectors.lineSize; j++)
                {
                    Sector currentSector = sectors.GetElement(i, j);
                    Biome biome = currentSector.GetBiomeList().GetFirst();

                    if (biome == null)
                    {
                        Debug.Log("Trying to add a None biome type in the BiomeManager\n"
                                  + "Position : " + i + ", " + j);
                    }
                    else
                    {
                        biome.Add(currentSector);
                    }
                }
            }
        }

        /// <summary>
        /// Organize every <see cref="Biome" />
        /// </summary>
        /// <param name="chunks">The chunk grid</param>
        private void OrganizeBiomes(FakeDoubleEntryList<Sector> sectors)
        {
            foreach (Biome biome in biomes)
            {
                biome.OrganizeLevel(sectors, (int)targetLevel);
            }
        }

        private Biome FindBiomeWithSample(float sample)
        {
            float biomeRatioValue = 0;

            foreach (Biome biome in biomes)
            {
                biomeRatioValue += biome.data.ratio;

                if (sample < biomeRatioValue)
                {
                    return biome;
                }
            }

            return biomes.Last();
        }

        public bool IsInitialized()
        {
            return _isInitiliazed;
        }

        public uint GetTargetLevel()
        {
            return targetLevel;
        }
    }
}