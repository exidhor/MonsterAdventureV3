using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.AI
{ 
    public class KinematicWander : KinematicSteering
    {
        private float _maxRotation;
        private float _maxOffsetChange;

        public void __KinematicWander__(float maxSpeed, float maxRotation, float maxOffsetChange)
        {
            __KinematicSteering__(maxSpeed);

            _maxRotation = maxRotation;
            _maxOffsetChange = maxOffsetChange;
        }

        public override void GiveSteering(ref SteeringOutput output, Kinematic character)
        {
            output.IsKinematic = true;
            output.IsOriented = true;

            // Move forward in the current direction
            output.Linear = character.GetOrientationAsVector();
            output.Linear *= GetMaxSpeed();

            // turn a little
            float change = RandomGenerator.Instance.NextBinomialFloat(_maxOffsetChange);
            output.AngularInDegree = change * _maxRotation;
        }

        public float GetMaxRotation()
        {
            return _maxRotation;
        }

        public float GetMaxOffsetChange()
        {
            return _maxOffsetChange;
        }
    }
}
