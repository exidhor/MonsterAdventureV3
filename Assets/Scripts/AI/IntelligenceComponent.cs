using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    [RequireComponent(typeof(Kinematic))]
    public class IntelligenceComponent : MonoBehaviour
    {
        // reference to set in the editor (easier to organized the object structure by this way)
        private Kinematic _kinematic;

        // get the data from the world
        private PerceptionComponent _perceptionComp;
        // find the goal from the data
        private Targeter _targeter;
        // blend the different steering
        private SteeringComponent _steeringComp;

        private void Awake()
        {
            _kinematic = GetComponent<Kinematic>();

            _perceptionComp = GetComponentInChildren<PerceptionComponent>();

            if (_perceptionComp == null)
            {
                Debug.LogWarning("No perception set for IntelligenceComponent on " + gameObject.name);
            }

            _targeter = GetComponent<Targeter>();
            _steeringComp = GetComponent<SteeringComponent>();
        }

        void OnEnable()
        {
            IntelligenceEngine.Instance.Register(this);
        }

        void OnDisable()
        {
            if (IntelligenceEngine.InternalInstance != null)
            {
                IntelligenceEngine.Instance.Unregister(this);
            }
        }

        /// <summary>
        /// Update and compute the steering without moving the object
        /// </summary>
        public void PreUpdate()
        {
            // actualize the known data from the world
            if(_perceptionComp != null)
                _perceptionComp.Actualize();

            // update the target from the data 
            if(_targeter != null)
                _targeter.Actualize();

            // update the steering
            if(_steeringComp != null)
                _steeringComp.ActualizeSteering(_kinematic);
        }

        /// <summary>
        /// Move the object (touch to the physics)
        /// </summary>
        public void PostUpdate(float deltaTime)
        {
            if(_steeringComp != null)
                _steeringComp.ApplySteeringOnKinematic(_kinematic, deltaTime);
        }
    }
}
