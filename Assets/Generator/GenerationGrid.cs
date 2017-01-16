using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure
{
    public class GenerationGrid
    {
        private GenerationToken[,] _tokens;
        private uint _level;

        public GenerationGrid(uint level, int generationId, Type typeValue)
        {
            _level = level;

            int size = (int)Math.Pow(2, _level);
            _tokens = new GenerationToken[size, size];

            for (int x = 0; x < _tokens.GetLength(0); x++)
            {
                for (int y = 0; y < _tokens.GetLength(1); y++)
                {
                    _tokens[x, y] = new GenerationToken(generationId, typeValue); 
                }
            }
        }
    }
}
