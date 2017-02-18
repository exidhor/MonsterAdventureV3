using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.Generation
{
    public abstract class ValueModifierData : GenerationData
    {
        public abstract GenerationData GetSource();
    }
}
