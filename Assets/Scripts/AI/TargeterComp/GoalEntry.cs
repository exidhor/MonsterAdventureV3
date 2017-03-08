﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.AI
{
    [Serializable]
    public class GoalEntry
    {
        public EGoalAction action;
        public int Priority;
        //public float Weight = 1;
        //public int GroupId = 0;
        public InGameId TargetValue;

        public List<Kinematic> TrackedTargets;
    }
}
