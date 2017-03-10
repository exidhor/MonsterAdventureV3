using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.AI
{
    [Serializable]
    public class InternalBehavior
    {
        public EBehavior Behavior;
        public float Weight;
        public List<Kinematic> Targets;
    }
}
