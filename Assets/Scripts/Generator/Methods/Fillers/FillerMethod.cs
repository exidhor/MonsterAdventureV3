using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.Generation
{
    public abstract class FillerMethod : GenerationMethod 
    {
        public FillerMethod(FillerData data, 
            GenerationTable generationTable, 
            GenerationType genererationType)
            : base(data, generationTable, genererationType, data.level)
        {
            // nothing ? 
        }
    }
}
