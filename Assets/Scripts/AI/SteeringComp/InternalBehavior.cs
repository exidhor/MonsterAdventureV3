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
        public List<Kinematic> Targets;
        public float Weight;

        public InternalBehavior(EBehavior behavior, List<Kinematic> targets, float weight)
        {
            Behavior = behavior;
            Targets = targets;
            Weight = weight;
        }
    }
}
