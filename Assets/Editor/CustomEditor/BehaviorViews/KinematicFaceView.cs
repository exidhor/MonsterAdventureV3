using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;

namespace MonsterAdventure.Editor
{
    public class KinematicFaceView : TargetedLocationSteeringView
    {
        protected override string GetTitle()
        {
            return "Face";
        }

        public override EBehavior GetBehavior()
        {
            return EBehavior.Face;
        }
        protected override void ApplyOn(AIComponent AIComponent)
        {
            AIComponent.AddFaceSteering(_pendingTargetLocation.ConstructLocation());
        }
    }
}
