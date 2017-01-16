using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure
{
    public class BiomeList
    {
        public int Count
        {
            get { return _biomeContainers.Count; }
        }

        private List<BiomeContainer> _biomeContainers;

        public BiomeList()
        {
            _biomeContainers = new List<BiomeContainer>();
        }

        public void AddBiome(Biome biome, float ratio)
        {
            int index = _biomeContainers.FindIndex(x => x.biome == biome);

            if (index == -1)
            {
                _biomeContainers.Add(new BiomeContainer(biome, ratio, GetTotalRatio()));
            }
            else
            {
                _biomeContainers[index].ratio += ratio;

                for (int i = index + 1; i < _biomeContainers.Count; i++)
                {
                    _biomeContainers[i].offsetRatio += ratio;
                }
            }
        }

        public List<BiomeContainer> GetBiomeContainers()
        {
            return _biomeContainers;
        }

        private float GetTotalRatio()
        {
            float totalRatio = 0;

            for (int i = 0; i < _biomeContainers.Count; i++)
            {
                totalRatio += _biomeContainers[i].ratio;
            }

            return totalRatio;
        }

        public Biome GetFirst()
        {
            if (_biomeContainers != null && _biomeContainers.Count >= 1)
                return _biomeContainers[0].biome;

            return null;
        }

        public bool Contains(Biome biome)
        {
            for (int i = 0; i < _biomeContainers.Count; i++)
            {
                if (_biomeContainers[i].biome == biome)
                {
                    return true;
                }
            }

            return false;
        }
    }
}