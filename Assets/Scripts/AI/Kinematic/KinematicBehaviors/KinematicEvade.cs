using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.AI
{
    public class KinematicEvade : InternalKinematicTargeted
    {
        private float _maxPredictionTime;

        private KinematicFlee _flee;

        public void __KinematicEvade__(float maxSpeed, Kinematic target, float maxPredictionTime)
        {
            __InternalKinematicTargeted__(maxSpeed, target);

            _flee = ConstructFlee();

            _flee.__KinematicFlee__(maxSpeed, _buffer);

            _maxPredictionTime = maxPredictionTime;
        }

        public override void GiveSteering(ref SteeringOutput output, Kinematic character)
        {
            if (_internalTarget == null)
                return;

            PredictTargetDeplacement(_maxPredictionTime, character);

            _flee.GiveSteering(ref output, character);
        }

        protected KinematicFlee ConstructFlee()
        {
            return (KinematicFlee) SteeringTable.Instance.GetFreeSteering(EBehavior.Flee);
        }

        public float GetMaxPredictionTime()
        {
            return _maxPredictionTime;
        }
    }
}