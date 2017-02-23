using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    public class KinematicArrive : TargetedKinematicSteering
    {
        private float _timeToTarget;
        private float _targetRadius;
        private float _slowRadius;

        public void __KinematicArrive__(float maxSpeed, Location target, float timeToTarget,
            float targetRadius, float slowRadius)
        {
            __TargetedKinematicSteering__(maxSpeed, target);

            _timeToTarget = timeToTarget;
            _targetRadius = targetRadius;
            _slowRadius = slowRadius;
        }

        protected override void GiveSteering(ref SteeringOutput output, Kinematic character)
        {
            output.IsKinematic = true;

            // First work out the direction
            output.Linear = GetTargetPosition();
            output.Linear -= character.GetPosition();

            float squareDistance = output.Linear.SqrMagnitude();

            // If there is no direction, do nothing
            if(squareDistance < _targetRadius * _targetRadius)
            {
                output.Linear = Vector2.zero;
            }
            else
            {
                output.Linear.Normalize();
                output.Linear *= GetMaxSpeed();

                // if we are outside the slowRadius, then go maxSpeed (and no changement)
                // Otherwise calculate a scaled speed
                if (squareDistance < _slowRadius*_slowRadius)
                {
                    output.Linear *= Mathf.Sqrt(squareDistance)/_slowRadius;
                }

                // Acceleration tries to get the target velocity
                //output.Linear -= character.GetVelocity();

                if(_timeToTarget > 0)
                    output.Linear /= _timeToTarget;

                // If that is too fast, then clip the speed
                output.Linear = Vector2.ClampMagnitude(output.Linear, GetMaxSpeed());
            }
        }
    }
}
