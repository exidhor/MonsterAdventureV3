using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    public abstract class TargetedKinematicSteering : KinematicSteering
    {
        public LocationComponent Target;

        protected virtual void Awake()
        {
            Target = new LocationComponent();
        } 

        protected void __TargetedKinematicSteering__(float maxSpeed, Location target)
        {
            __KinematicSteering__(maxSpeed);

            SetTargetLocation(target);
        }

        public void SetTargetLocation(Location location)
        {
            Target.SetLocation(location);
        }

        public Vector2 GetTargetPosition()
        {
            return Target.GetPosition();
        }

        public LocationComponent GetTargetLocationComponent()
        {
            return Target;
        }
    }
}