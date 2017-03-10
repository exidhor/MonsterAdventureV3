using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    [RequireComponent(typeof(PerceptionComponent), typeof(Targeter), typeof(SteeringComponent))]
    public class IntelligenceComponent : MonoBehaviour
    {
        // reference to set in the editor (easier to organized the object structure by this way)
        public Kinematic Kinematic;

        // get the data from the world
        private PerceptionComponent _percetionComp;
        // find the goal from the data
        private Targeter _targeter;
        // blend the different steering
        private SteeringComponent _steeringComp;

        private void Awake()
        {
            _percetionComp = GetComponent<PerceptionComponent>();
            _targeter = GetComponent<Targeter>();
            _steeringComp = GetComponent<SteeringComponent>();

            if (Kinematic == null)
            {
                Debug.LogError("No kinematic set for IntelligenceComponent on " + gameObject.name);
            }
        }

        private void FixedUpdate()
        {
            // todo : remove this, it's temporary
            PreUpdate();
            PostUpdate();
        }

        /// <summary>
        /// Update and compute the steering without moving the object
        /// </summary>
        private void PreUpdate()
        {
            // actualize the known data from the world
            _percetionComp.Actualize();

            // update the target from the data 
            _targeter.Actualize();

            // update the steering
           _steeringComp.Actualize(Kinematic, Time.fixedDeltaTime);
        }

        /// <summary>
        /// Move the object (touch to the physics)
        /// </summary>
        private void PostUpdate()
        {
            // todo : apply the steerings on the kinematic body
        }
    }
}
