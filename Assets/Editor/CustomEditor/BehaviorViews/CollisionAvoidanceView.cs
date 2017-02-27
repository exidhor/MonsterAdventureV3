using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;

namespace MonsterAdventure.Editor
{
    public class CollisionAvoidanceView : KinematicSteeringView
    {
        protected override string GetTitle()
        {
            return "Collision Avoidance";
        }

        public override EBehavior GetBehavior()
        {
            return EBehavior.CollisionAvoidance;
        }

        protected override void ApplyOn(AIComponent AIComponent)
        {
            
        }
    }
}
