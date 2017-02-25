using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;

namespace MonsterAdventure.Editor
{
    public class LocationSeekView : TargetedLocationSteeringView
    {
        //private KinematicSeek _seek; 
             
        //public KinematicSeekView(KinematicSeek steering)
        //{
        //    _seek = steering;

        //    Initialize();
        //}

        protected override string GetTitle()
        {
            return "Seek";
        }

        public override EBehavior GetBehavior()
        {
            return EBehavior.Seek;
        }

        protected override void ApplyOn(AIComponent AIComponent)
        {
            AIComponent.AddSeekSteering(_pendingMaxSpeed.Value, _pendingTargetLocation.ConstructLocation());
        }

        public virtual void Actualize(KinematicSteering steering)
        {
            base.Actualize(steering);

            KinematicSeek seek = (KinematicSeek) steering;

            // nothing
        }

        //protected override TargetedLocationSteering GetTargetedKinematicSteering()
        //{
        //    return _seek;
        //}
    }
}
