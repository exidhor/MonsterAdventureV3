using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;
using UnityEditor;

namespace MonsterAdventure.Editor
{
    public abstract class InternalKinematicTargetView : KinematicSteeringView
    {
        protected KinematicField _pendingKinematicTarget;

        public InternalKinematicTargetView()
        {
            _pendingKinematicTarget = new KinematicField(null);
        }

        protected override void DisplayContent()
        {
            base.DisplayContent();

            _pendingKinematicTarget.DisplayOnGUI("KinematicTarget");
        }

        protected override void RevertFrom(KinematicSteering steering)
        {
            base.RevertFrom(steering);

            InternalKinematicTargeted internalKinematicTargeted = (InternalKinematicTargeted) steering;

            _pendingKinematicTarget.SetValue(internalKinematicTargeted.GetTargetKinematic());
        }
    }
}
