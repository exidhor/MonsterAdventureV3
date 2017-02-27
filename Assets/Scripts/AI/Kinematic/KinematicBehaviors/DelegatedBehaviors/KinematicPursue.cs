using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.AI
{
    public class KinematicPursue : InternalKinematicTargeted
    {
        private float _maxPredictionTime;

        private KinematicSeek _seek;

        public void __KinematicPursue__(float maxSpeed, Kinematic target, float maxPredictionTime)
        {
            __InternalKinematicTargeted__(maxSpeed, target);

            _seek = ConstructSeek();

            _seek.__KinematicSeek__(maxSpeed, _buffer);

            _maxPredictionTime = maxPredictionTime;
        }

        public override void GiveSteering(ref SteeringOutput output, Kinematic character)
        {
            if(_internalTarget == null)
                return;
            
            PredictTargetDeplacement(_maxPredictionTime, character);

            _seek.GiveSteering(ref output, character);
        }

        protected KinematicSeek ConstructSeek()
        {
            return (KinematicSeek) SteeringTable.Instance.GetFreeSteering(EBehavior.Seek);
        }

        public float GetMaxPredictionTime()
        {
            return _maxPredictionTime;
        }
    }
}
