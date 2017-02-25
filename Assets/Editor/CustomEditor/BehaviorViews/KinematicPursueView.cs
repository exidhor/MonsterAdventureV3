using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;

namespace MonsterAdventure.Editor
{
    public class KinematicPursueView : InternalKinematicTargetView
    {
        private FloatField _pendingMaxPredictionTime;

        public KinematicPursueView()
        {
            _pendingMaxPredictionTime = new FloatField(0);
        }

        protected override string GetTitle()
        {
            return "Pursue";
        }

        public override EBehavior GetBehavior()
        {
            return EBehavior.Pursue;
        }

        protected override void DisplayContent()
        {
            base.DisplayContent();

            _pendingMaxPredictionTime.DisplayOnGUI("Max Prediction Time");
        }

        protected override void ApplyOn(AIComponent AIComponent)
        {
            AIComponent.AddPursueSteering(_pendingMaxSpeed.Value,
                _pendingKinematicTarget.Value,
                _pendingMaxPredictionTime.Value);
        }

        protected override void RevertFrom(KinematicSteering steering)
        {
            base.RevertFrom(steering);

            KinematicPursue pursue = (KinematicPursue) steering;

            _pendingMaxPredictionTime.SetValue(pursue.GetMaxPredictionTime());
        }
    }
}
