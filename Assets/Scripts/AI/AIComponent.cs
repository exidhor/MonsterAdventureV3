using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    [RequireComponent(typeof(Kinematic))]
    public class AIComponent : MonoBehaviour
    {
        public EBehavior Behavior = EBehavior.None;
        public SteeringOutput OutputBuffer;

        private Kinematic _kinematic;
        private KinematicSteering _steering;

        private int _instanceIdInScene;

        private void Awake()
        {
            _instanceIdInScene = GetInstanceID();
        }

        private void OnEnable()
        {
            AITable.Instance.Register(this);
        }

        private void OnDisable()
        {
            if (AITable.NotModifiedInstance != null)
            {
                AITable.Instance.Unregister(this.GetInstanceID());
            }
        }

        private void Start()
        {
            _kinematic = GetComponent<Kinematic>();

            OutputBuffer = new SteeringOutput();

            ConstructKinematicSteering();
        }

        private void FixedUpdate()
        {
            ConstructKinematicSteering();

            if (_steering != null)
            {
                _steering.GiveSteering(ref OutputBuffer, _kinematic);
            }

            _kinematic.Actualize(Time.fixedDeltaTime, OutputBuffer);
        }

        public int GetInstanceIdInScene()
        {
            return _instanceIdInScene;
        }

        public void SetKinematicSteering(KinematicSteering kinematicSteering)
        {
            _steering = kinematicSteering;
        }

        public void ConstructKinematicSteering()
        {
            switch (Behavior)
            {
                case EBehavior.None:
                    _steering = null;
                    break;

                case EBehavior.Seek:
                    _steering = new KinematicSeek(1, new StationaryLocation(10, 10));
                    break;
            }
        }

        public KinematicSteering GetSteering()
        {
            return _steering;
        }
    }
}
