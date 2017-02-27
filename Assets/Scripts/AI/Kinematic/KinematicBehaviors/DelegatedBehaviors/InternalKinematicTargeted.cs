using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    public abstract class InternalKinematicTargeted : KinematicSteering
    {
        public float prediction;
        public float distance;

        protected Kinematic _internalTarget;

        [SerializeField]
        protected StationaryLocation _buffer;

        public void __InternalKinematicTargeted__(float maxSpeed, Kinematic target)
        {
            __KinematicSteering__(maxSpeed);

            _buffer = target.GetInstantLocation();

            _internalTarget = target;
        }

        protected void PredictTargetDeplacement(float maxPredictionTime, Kinematic character)
        {
            // Work out the distance to target
            Vector2 direction = _internalTarget.GetPosition() - character.GetPosition();
            
            //float distance = direction.magnitude;
            distance = direction.magnitude;

            // Work out our current speed
            float currentSpeed = character.GetVelocity().magnitude;

            //float prediction = maxPredictionTime;
            prediction = maxPredictionTime;

            // Check if speed is too small to give a reasonable prediction time
            // Otherwise calculate the prediction time
            if (currentSpeed > distance/maxPredictionTime)
            {
                prediction = distance / currentSpeed;
            }

            // Put the target together
            Vector2 targetPosition = _internalTarget.GetPosition();
            targetPosition += _internalTarget.GetVelocity() * prediction;

            _buffer.SetPosition(targetPosition);
        }

        public Kinematic GetTargetKinematic()
        {
            return _internalTarget;
        }
    }
}
