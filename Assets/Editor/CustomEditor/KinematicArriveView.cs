using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;

namespace MonsterAdventure.Editor
{
    public class KinematicArriveView : TargetedKinematicSteeringView
    {
        private FloatField _pendingTimeToTarget;
        private FloatField _pendingTargetRadius;
        private FloatField _pendingSlowRadius;

        public KinematicArriveView()
        {
            _pendingTimeToTarget = new FloatField(0);
            _pendingTargetRadius = new FloatField(0);
            _pendingSlowRadius = new FloatField(0);
        }

        protected override string GetTitle()
        {
            return "Arrive";
        }

        protected override void DisplayContent()
        {
            base.DisplayContent();

            _pendingTimeToTarget.DisplayOnGUI("Time To Target");
            _pendingTargetRadius.DisplayOnGUI("Target Radius");
            _pendingSlowRadius.DisplayOnGUI("Slow Radius");
        }

        public override EBehavior GetBehavior()
        {
            return EBehavior.Arrive;
        }

        protected override void ApplyOn(AIComponent AIComponent)
        {
            AIComponent.AddArriveSteering(_pendingMaxSpeed.Value,
                _pendingTargetLocation.ConstructLocation(), 
                _pendingTimeToTarget.Value,
                _pendingTargetRadius.Value,
                _pendingSlowRadius.Value);
        }
    }
}
