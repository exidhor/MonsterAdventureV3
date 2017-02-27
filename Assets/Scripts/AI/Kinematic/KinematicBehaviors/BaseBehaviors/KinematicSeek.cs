using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    public class KinematicSeek : TargetedLocationSteering
    {
        public void __KinematicSeek__(float maxSpeed, Location target)
        {
            __TargetedLocationSteering__(maxSpeed, target);
        }

        public override void GiveSteering(ref SteeringOutput output, Kinematic character)
        {
            // First work out the direction
            output.Linear = GetTargetPosition();
            output.Linear -= character.GetPosition();
            
            // If there is no direction, do nothing
            output.Linear = MathHelper.GetKinematicMovement_MinCheck(output.Linear, GetMaxSpeed(), 0.01f);
        }
    }
}
