using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.AI
{
    public abstract class TargetedKinematicSteering : KinematicSteering
    {
        public Location Target;

        public TargetedKinematicSteering(float maxSpeed, Location target)
            : base(maxSpeed)
        {
            Target = target;
        }

        public void SetTargetLocation(Location location)
        {
            Target = location;
        }

        public Location GetTargetLocation()
        {
            return Target;
        }
    }
}