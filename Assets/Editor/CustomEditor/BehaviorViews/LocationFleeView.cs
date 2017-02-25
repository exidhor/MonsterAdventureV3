using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;

namespace MonsterAdventure.Editor
{
    public class LocationFleeView : LocationSeekView
    {
        protected override string GetTitle()
        {
            return "Flee";
        }

        public override EBehavior GetBehavior()
        {
            return EBehavior.Flee;
        }

        protected override void ApplyOn(AIComponent AIComponent)
        {
            AIComponent.AddFleeSteering(_pendingMaxSpeed.Value, _pendingTargetLocation.ConstructLocation());
        }
    }
}
