using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.AI
{
    public class KinematicFlee : KinematicSeek
    {
        public void __KinematicFlee__(float maxSpeed, Location target)
        {
            __KinematicSeek__(maxSpeed, target);
        }

        public override void GiveSteering(ref SteeringOutput output, Kinematic character)
        {
            // First work out the direction
            output.Linear = character.GetPosition();
            output.Linear -= GetTargetPosition();

            // If there is no direction, do nothing
            output.Linear = MathHelper.GetKinematicMovement_MinCheck(output.Linear, GetMaxSpeed(), 0.01f);
        }
    }
}