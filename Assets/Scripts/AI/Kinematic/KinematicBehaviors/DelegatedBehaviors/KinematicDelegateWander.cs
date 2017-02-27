using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    public class KinematicDelegateWander : KinematicFace
    {
        private float _wanderOffset;
        private float _wanderRadius;
        private float _wanderRate;

        private float _wanderOrientationInDegree;

        public void __KinematicDelegateWander__(float maxSpeed,
            float wanderOffset,
            float wanderRadius,
            float wanderRate)
        {
            _wanderOffset = wanderOffset;
            _wanderRadius = wanderRadius;
            _wanderRate = wanderRate;

            // just a buffer into the steering to hol dthe current orientation
            // of the wander target
            _wanderOrientationInDegree = 0;

            StationaryLocation targetLocation = new StationaryLocation(Vector2.zero);

            __KinematicFace__(maxSpeed, targetLocation);
        }

        public override void GiveSteering(ref SteeringOutput output, Kinematic character)
        {
            output.IsOriented = true;

            // calculate the target to delegate to face

            // update the wanderorientation
            _wanderOrientationInDegree += RandomGenerator.Instance.NextBinomialFloat(1) * _wanderRate;

            // calculate the combined target orientation
            float targetOrientationInDegree = _wanderOrientationInDegree + character.OrientationInDegree;

            Vector2 characterOrientationAsVector = character.GetOrientationAsVector();

            // calculate the center of the wander circle
            Vector2 targetPosition = character.GetPosition()
                + _wanderOffset * characterOrientationAsVector;

            // calculate the target location
            targetPosition += _wanderRadius * MathHelper.GetDirectionFromAngle(
                targetOrientationInDegree * Mathf.Deg2Rad);

            SetTargetPosition(targetPosition);

            // delegate to face
            base.GiveSteering(ref output, character);

            // now set the speed
            output.Linear = GetMaxSpeed() * characterOrientationAsVector;
        }

        public float GetWanderOffset()
        {
            return _wanderOffset;
        }

        public float GetWanderRadius()
        {
            return _wanderRadius;
        }

        public float GetWanderRate()
        {
            return _wanderRate;
        }

        public float GetWanderOrientationInDegree()
        {
            return _wanderOrientationInDegree;
        }
    }
}
