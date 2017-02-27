using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;

namespace MonsterAdventure.Editor
{
    public class KinematicDelegateWanderView : KinematicSteeringView
    {
        private FloatField _pendingWanderOffset;
        private FloatField _pendingWanderRadius;
        private FloatField _pendingWanderRate;

        public KinematicDelegateWanderView()
        {
            _pendingWanderOffset = new FloatField(0);
            _pendingWanderRadius = new FloatField(0);
            _pendingWanderRate = new FloatField(0);
        }

        protected override string GetTitle()
        {
            return "Delegate Wander";
        }

        public override EBehavior GetBehavior()
        {
            return EBehavior.DelegateWander;
        }

        protected override void DisplayContent()
        {
            base.DisplayContent();

            _pendingWanderOffset.DisplayOnGUI("Wander Offset");
            _pendingWanderRadius.DisplayOnGUI("Wander Radius");
            _pendingWanderRate.DisplayOnGUI("Wander Rate");
        }

        protected override void ApplyOn(AIComponent AIComponent)
        {
            AIComponent.AddDelegateWanderSteering(_pendingMaxSpeed.Value,
                _pendingWanderOffset.Value,
                _pendingWanderRadius.Value,
                _pendingWanderRate.Value);
        }

        protected override void RevertFrom(KinematicSteering steering)
        {
            base.RevertFrom(steering);

            KinematicDelegateWander delegateWander = (KinematicDelegateWander) steering;

            _pendingWanderOffset.SetValue(delegateWander.GetWanderOffset());
            _pendingWanderRadius.SetValue(delegateWander.GetWanderRadius());
            _pendingWanderRate.SetValue(delegateWander.GetWanderRate());
        }
    }
}
