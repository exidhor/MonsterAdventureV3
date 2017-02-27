using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    public abstract class TargetedLocationSteering : KinematicSteering
    {
        public LocationComponent Target;

        protected virtual void Awake()
        {
            Target = new LocationComponent();
        } 

        public void __TargetedLocationSteering__(float maxSpeed, Location target)
        {
            __KinematicSteering__(maxSpeed);

            SetTargetLocation(target);
        }

        public void SetTargetLocation(Location location)
        {
            Target.SetLocation(location);
        }

        public void SetTargetPosition(Vector2 targetPosition)
        {
            Target.SetTargetPosition(targetPosition);
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