using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.Generation
{
    public abstract class GridModifierData : GenerationData
    {
        public abstract GenerationData GetSource();
    }
}
