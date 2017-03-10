using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    public static partial class Behavior
    {
        public static void Wander(ref SteeringOutput output, Kinematic character, SteeringSpecs specs)
        {
            output.IsOriented = true;

            // calculate the target to delegate to face

            // update the wanderorientation
            specs.WanderOrientation += RandomGenerator.Instance.NextBinomialFloat(1) * specs.WanderRate;

            // calculate the combined target orientation
            float targetOrientationInDegree = specs.WanderOrientation + character.OrientationInDegree;

            Vector2 characterOrientationAsVector = character.GetOrientationAsVector();

            // calculate the center of the wander circle
            Vector2 targetPosition = character.GetPosition() + specs.MaxWanderOffset * characterOrientationAsVector;

            // calculate the target location
            targetPosition += specs.WanderRadius * MathHelper.GetDirectionFromAngle(
                targetOrientationInDegree * Mathf.Deg2Rad);

            Face(ref output, character, specs, targetPosition);

            // now set the speed
            output.Linear = specs.MaxSpeed * characterOrientationAsVector;
        }
    }
}
