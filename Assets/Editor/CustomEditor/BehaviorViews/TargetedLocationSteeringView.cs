using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;

namespace MonsterAdventure.Editor
{
    public abstract class TargetedLocationSteeringView : KinematicSteeringView
    {
        protected SerializableLocation _pendingTargetLocation;

        protected TargetedLocationSteeringView()
        {
            _pendingTargetLocation = new SerializableLocation();
        }

        protected override void DisplayContent()
        {
            base.DisplayContent();

            if (_pendingTargetLocation != null)
            {
                _pendingTargetLocation.DisplayOnGUI("Target Location");
            }
        }

        public virtual void Actualize(KinematicSteering steering)
        {
            base.Actualize(steering);

            TargetedLocationSteering targetedLocationSteering = (TargetedLocationSteering) steering;
            _pendingTargetLocation.Actualize(targetedLocationSteering.GetTargetLocationComponent());
        }

        //protected override void ApplyOn(KinematicSteering steering)
        //{
        //    base.ApplyOn(steering);

        //    TargetedLocationSteering targetedSteering = (TargetedLocationSteering) steering;

        //    targetedSteering.SetTargetLocation(_pendingTargetLocation.ConstructLocation());

        //    //GetTargetedKinematicSteering().SetTargetLocation(TargetLocation.ConstructLocation());
        //}

        protected override void RevertFrom(KinematicSteering steering)
        {
            base.RevertFrom(steering);

            TargetedLocationSteering targetedLocationSteering = (TargetedLocationSteering)steering;

            _pendingTargetLocation.Actualize(targetedLocationSteering.GetTargetLocationComponent());

            //TargetLocation.Initialize(GetTargetedKinematicSteering().GetTargetLocationComponent());
        }

        //protected abstract TargetedLocationSteering GetTargetedKinematicSteering();

        //protected override KinematicSteering GetSteering()
        //{
        //    return GetTargetedKinematicSteering();
        //}
    }
}
