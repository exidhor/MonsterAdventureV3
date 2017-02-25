using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;

namespace MonsterAdventure.Editor
{
    public class KinematicEvadeView : InternalKinematicTargetView
    {
        private FloatField _pendingMaxPredictionTime;

        public KinematicEvadeView()
        {
            _pendingMaxPredictionTime = new FloatField(0);
        }

        protected override string GetTitle()
        {
            return "Evade";
        }

        public override EBehavior GetBehavior()
        {
            return EBehavior.Evade;
        }

        protected override void DisplayContent()
        {
            base.DisplayContent();

            _pendingMaxPredictionTime.DisplayOnGUI("Max Prediction Time");
        }

        protected override void ApplyOn(AIComponent AIComponent)
        {
            AIComponent.AddEvadeSteering(_pendingMaxSpeed.Value,
                _pendingKinematicTarget.Value,
                _pendingMaxPredictionTime.Value);
        }

        protected override void RevertFrom(KinematicSteering steering)
        {
            base.RevertFrom(steering);

            KinematicEvade evade = (KinematicEvade)steering;

            _pendingMaxPredictionTime.SetValue(evade.GetMaxPredictionTime());
        }
    }
}
