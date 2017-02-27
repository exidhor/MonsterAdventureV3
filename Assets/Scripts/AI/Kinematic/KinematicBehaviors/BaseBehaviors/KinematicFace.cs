using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    public class KinematicFace : TargetedLocationSteering
    {
        // usefull for inheritance 
        protected void __KinematicFace__(float maxSpeed, Location target)
        {
            __TargetedLocationSteering__(maxSpeed, target);
        }

        public void __KinematicFace__(Location target)
        {
            // we dont care about "maxSpeed" in face
            __KinematicFace__(0, target);
        }

        public override void GiveSteering(ref SteeringOutput output, Kinematic character)
        {
            output.IsInstantOrientation = true;

            // work out the direction to target
            Vector2 direction = GetTargetPosition() - character.GetPosition();
            
            // Check for a zero direction, and make no change if so
            if (direction.sqrMagnitude == 0)
            {
                return;
            }

            output.AngularInDegree = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        }
    }
}