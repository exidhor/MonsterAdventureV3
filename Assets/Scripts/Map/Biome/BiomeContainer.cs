using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure
{
    public class BiomeContainer
    {
        public Biome biome;
        public float ratio;
        public float offsetRatio;

        public BiomeContainer(Biome biome, float ratio, float offsetRatio)
        {
            this.biome = biome;
            this.ratio = ratio;
            this.offsetRatio = offsetRatio;
        }
    }
}