using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;

namespace MonsterAdventure.Editor
{
    public abstract class TargetedKinematicSteeringView : KinematicSteeringView
    {
        protected SerializableLocation _pendingTargetLocation;

        protected TargetedKinematicSteeringView()
        {
            _pendingTargetLocation = new SerializableLocation();
        }

        protected override void Initialize()
        {
            base.Initialize();

            //TargetLocation = new SerializableLocation(GetTargetedKinematicSteering().Target);
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

            TargetedKinematicSteering targetedSteering = (TargetedKinematicSteering) steering;
            _pendingTargetLocation.Actualize(targetedSteering.GetTargetLocationComponent());
        }

        //protected override void ApplyOn(KinematicSteering steering)
        //{
        //    base.ApplyOn(steering);

        //    TargetedKinematicSteering targetedSteering = (TargetedKinematicSteering) steering;

        //    targetedSteering.SetTargetLocation(_pendingTargetLocation.ConstructLocation());

        //    //GetTargetedKinematicSteering().SetTargetLocation(TargetLocation.ConstructLocation());
        //}

        protected override void RevertFrom(KinematicSteering steering)
        {
            base.RevertFrom(steering);

            TargetedKinematicSteering targetedSteering = (TargetedKinematicSteering)steering;

            _pendingTargetLocation.Actualize(targetedSteering.GetTargetLocationComponent());

            //TargetLocation.Initialize(GetTargetedKinematicSteering().GetTargetLocationComponent());
        }

        //protected abstract TargetedKinematicSteering GetTargetedKinematicSteering();

        //protected override KinematicSteering GetSteering()
        //{
        //    return GetTargetedKinematicSteering();
        //}
    }
}
