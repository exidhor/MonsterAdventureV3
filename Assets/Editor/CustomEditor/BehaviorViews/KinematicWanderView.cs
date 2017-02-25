using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;
using MonsterAdventure.Editor;

namespace MonsterAdventure.Editor
{
    class KinematicWanderView : KinematicSteeringView
    {
        private FloatField _pendingMaxRotation;
        private FloatField _pendingMaxOffsetChange;

        public KinematicWanderView()
        {
            _pendingMaxRotation = new FloatField(0);
            _pendingMaxOffsetChange = new FloatField(0);
        }

        protected override void DisplayContent()
        {
            base.DisplayContent();

            _pendingMaxRotation.DisplayOnGUI("Max Rotation");
            _pendingMaxOffsetChange.DisplayOnGUI("Max Offset Change");
        }

        protected override string GetTitle()
        {
            return "Wander";
        }

        public override EBehavior GetBehavior()
        {
            return EBehavior.Wander;
        }

        protected override void ApplyOn(AIComponent AIComponent)
        {
            AIComponent.AddWanderSteering(_pendingMaxSpeed.Value,
                _pendingMaxRotation.Value,
                _pendingMaxOffsetChange.Value);
        }

        protected override void RevertFrom(KinematicSteering steering)
        {
            base.RevertFrom(steering);

            KinematicWander wander = (KinematicWander) steering;

            _pendingMaxRotation.SetValue(wander.GetMaxRotation());
            _pendingMaxOffsetChange.SetValue(wander.GetMaxOffsetChange());
        }
    }
}
