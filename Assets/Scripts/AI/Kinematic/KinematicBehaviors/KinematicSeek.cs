using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    public class KinematicSeek : TargetedKinematicSteering
    {
        public void __KinematicSeek__(float maxSpeed, Location target)
        {
            __TargetedKinematicSteering__(maxSpeed, target);
        }

        protected override void GiveSteering(ref SteeringOutput output, Kinematic character)
        {
            //if (GetTargetLocation() == null)
            //{
            //    output.IsKinematic = true;
            //    output.Linear = Vector2.zero;

            //    return;
            //}

            // First work out the direction
            output.IsKinematic = true;
            output.Linear = GetTargetPosition();
            output.Linear -= character.GetPosition();
            
            // If there is no direction, do nothing
            output.Linear = MathHelper.GetKinematicMovement_MinCheck(output.Linear, GetMaxSpeed(), 0.01f);
            //output.Linear = Vector2.ClampMagnitude(output.Linear, GetMaxSpeed());
        }
    }
}
