using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.AI
{
    /// <summary>
    /// It's more than just a movement behavior (for example eat means also
    /// an change in animation and internal state)
    /// </summary>
    public enum EGoalAction
    {
        None,
        Wander,
        Flee,
        Pursue,
        Eat, 
        Attack
    }
}
