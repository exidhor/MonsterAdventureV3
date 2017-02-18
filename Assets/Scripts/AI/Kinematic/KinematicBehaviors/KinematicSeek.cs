using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    public class KinematicSeek : TargetedKinematicSteering
    {
        public KinematicSeek(float maxSpeed, Location target)
            : base(maxSpeed, target)
        {
            // nothing
        }

        public override void GiveSteering(ref SteeringOutput output, Kinematic character)
        {
            // First work out the direction
            output.IsKinematic = true;
            output.Linear = GetTargetLocation().GetPosition();
            output.Linear -= character.GetPosition();
            
            // If there is no direction, do nothing
            output.Linear = Vector2.ClampMagnitude(output.Linear, GetMaxSpeed());
        }
    }
}
