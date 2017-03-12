﻿using System;
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
            _percetionComp.Actualize();

            // update the target from the data 
            _targeter.Actualize();

            // update the steering
           _steeringComp.ActualizeSteering(Kinematic);
        }

        /// <summary>
        /// Move the object (touch to the physics)
        /// </summary>
        public void PostUpdate(float deltaTime)
        {
            _steeringComp.ApplySteeringOnKinematic(Kinematic, deltaTime);
        }
    }
}
