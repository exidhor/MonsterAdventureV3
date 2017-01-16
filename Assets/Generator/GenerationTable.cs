using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure
{
    public class GenerationTable
    {
        private List<GenerationMethod> _table;

        public GenerationTable()
        {
            _table = new List<GenerationMethod>();
        }

        public int Register(GenerationMethod generationMethod)
        {
            _table.Add(generationMethod);

            return _table.Count - 1;
        }

        public GenerationMethod Get(int id)
        {
            return _table[id];
        }
    }
}
