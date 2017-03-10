using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    public static partial class Behavior
    {
        public static void Face(ref SteeringOutput output, Kinematic character, SteeringSpecs specs, Vector2 target)
        {
            output.IsInstantOrientation = true;

            // work out the direction to target
            Vector2 direction = target - character.GetPosition();

            // Check for a zero direction, and make no change if so
            if (direction.sqrMagnitude < float.Epsilon*float.Epsilon)
            {
                return;
            }

            output.AngularInDegree = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        }
    }
}
