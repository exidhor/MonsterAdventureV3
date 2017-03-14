using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.AI
{
    [Serializable]
    public class GoalEntry
    {
        public EBehavior behavior;
        public int Priority;
        public float Weight = 1;
        public string GroupKey = "";

        public bool IsDefault = false;

        public InGameId TargetValue;
        public List<Presence> TrackedTargets;
    }
}
