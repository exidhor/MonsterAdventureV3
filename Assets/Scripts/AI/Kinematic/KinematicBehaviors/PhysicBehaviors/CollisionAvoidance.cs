using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    public class CollisionAvoidance : KinematicSteering
    {
        private List<Kinematic> _targets;

        private float _radius;

        public void __CollisionAvoidance__(float maxSpeed, float radius)
        {
            __KinematicSteering__(maxSpeed);

            _radius = radius;

            _targets = new List<Kinematic>();
        }

        public void SetTargets(List<Kinematic> targets)
        {
            _targets = targets;
        }

        public override void GiveSteering(ref SteeringOutput output, Kinematic character)
        {
            // 1 - Find the target that's closest to collision

            // store the first collision time
            float shortestTime = float.MaxValue;

            // Store the target that collides then, and other data
            // that we will need and can avoid recalculating
            Kinematic firstTarget = null;
            float firstMinSperation = 0;
            float firstDistance = 0;
            Vector2 firstRelativePos = Vector2.zero;
            Vector2 firstRelativeVel = Vector2.zero;

            // loop through each target
            for (int i = 0; i < _targets.Count; i++)
            {
                Vector2 relativePos = _targets[i].GetPosition() - character.GetPosition();
                Vector2 relativeVel = _targets[i].GetVelocity() - character.GetVelocity();
                float relativeSpeed = relativeVel.magnitude;

                float timeToCollision = Vector2.Dot(relativePos, relativeVel) / (relativeSpeed * relativeSpeed);

                // check if it is going to be a collision at all
                float distance = relativePos.magnitude;
                float minSeparation = distance - relativeSpeed*timeToCollision;

                if (minSeparation > 2*_radius)
                {
                    continue;
                }

                // check if it is the shortest
                if (timeToCollision > 0 && timeToCollision < shortestTime)
                {
                    // store the time, target and other data
                    shortestTime = timeToCollision;
                    firstTarget = _targets[i];
                    firstMinSperation = minSeparation;
                    firstDistance = distance;
                    firstRelativePos = relativePos;
                    firstRelativeVel = relativeVel;
                }
            }

            // 2 - Calculate the steering

            // if we have no target, then exit
            if (firstTarget == null)
            {
                return;
            }

            Vector2 relativePosWithTarget = firstRelativePos;

            // if we're going to hit exactly, or if we're already colliding
            // then do the steering based on current position
            if (firstMinSperation <= 0 || firstDistance < 2*_radius)
            {
                // nothing
            }
            else // otherwise calculate the future relative position
            {
                relativePosWithTarget += firstRelativeVel*shortestTime;
            }

            // Avoid the target
            relativePosWithTarget.Normalize();
            output.Linear = relativePosWithTarget * GetMaxSpeed();
        }

        public float GetRadius()
        {
            return _radius;
        }
    }
}
