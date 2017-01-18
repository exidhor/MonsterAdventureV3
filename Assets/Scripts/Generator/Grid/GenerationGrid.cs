using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.Generation
{
    public class GenerationGrid
    {
        private GenerationToken[,] _tokens;
        private uint _level;

        private int _generationId;

        public int lineSize
        {
            get { return _tokens.GetLength(0); }
        }

        public GenerationGrid(uint level, int generationId)
        {
            _generationId = generationId;

            _level = level;

            int size = (int) Math.Pow(2, _level);
            _tokens = new GenerationToken[size, size];

            for (int x = 0; x < _tokens.GetLength(0); x++)
            {
                for (int y = 0; y < _tokens.GetLength(1); y++)
                {
                    _tokens[x, y] = new GenerationToken();
                }
            }
        }

        public GenerationToken Get(int x, int y)
        {
            return _tokens[x, y];
        }

        public uint GetLevel()
        {
            return _level;
        }
    }
}