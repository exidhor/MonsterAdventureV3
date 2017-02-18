using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.AI
{
    public abstract class KinematicSteering
    {
        private float _maxSpeed;

        public abstract void GiveSteering(ref SteeringOutput output, Kinematic character);

        public KinematicSteering(float maxSpeed)
        {
            _maxSpeed = maxSpeed;
        }

        public void SetMaxSpeed(float maxSpeed)
        {
            _maxSpeed = maxSpeed;
        }

        public float GetMaxSpeed()
        {
            return _maxSpeed;
        }
    }
}
