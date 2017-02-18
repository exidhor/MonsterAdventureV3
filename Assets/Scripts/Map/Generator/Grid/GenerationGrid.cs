using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Debug = UnityEngine.Debug;

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

            InitWith(-1);
        }

        public void InitWith(float value)
        {
            for (int i = 0; i < _tokens.GetLength(0); i++)
            {
                for (int j = 0; j < _tokens.GetLength(1); j++)
                {
                    _tokens[i, j].SetValue(value);
                }
            }
        }

        public GenerationToken Get(int x, int y)
        {
            if (x < 0 || y < 0
                || x >= _tokens.GetLength(0) || y >= _tokens.GetLength(1))
            {
                Debug.LogWarning("Try to access at an invalid index (" + x + ", " + y + ")");
            }

            return _tokens[x, y];
        }

        public GenerationToken Get(Coords coords)
        {
            return _tokens[coords.abs, coords.ord];
        }

        public List<GenerationToken> ToList()
        {
            List<GenerationToken> gridList = new List<GenerationToken>(_tokens.Length);

            for (int i = 0; i < _tokens.GetLength(0); i++)
            {
                for (int j = 0; j < _tokens.GetLength(1); j++)
                {
                    gridList.Add(_tokens[i, j]);
                }
            }

            return gridList;
        }

        public uint GetLevel()
        {
            return _level;
        }
    }
}