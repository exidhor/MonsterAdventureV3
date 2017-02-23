using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;
using UnityEngine;

namespace MonsterAdventure.AI
{
    [Serializable]
    public class SteeringEntry : PoolEntry
    {
        public EBehavior Behavior;
    }
}
