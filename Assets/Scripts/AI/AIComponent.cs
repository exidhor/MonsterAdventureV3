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
        private EBehavior _oldBehavior = EBehavior.None;    
        public SteeringOutput OutputBuffer;

        private Kinematic _kinematic;
        private KinematicSteering _steering;

        private int _instanceIdInScene;

        private void Awake()
        {
            _instanceIdInScene = GetInstanceID();
        }

        private void Start()
        {
            _kinematic = GetComponent<Kinematic>();

            OutputBuffer = new SteeringOutput();

            _steering = null;

            //ApplyKinematicSteering();
        }

        private void Update()
        {
            //ApplyKinematicSteering();
        }

        private void FixedUpdate()
        {
            //ApplyKinematicSteering();

            if (_steering != null)
            {
                _steering.ConfigureSteeringOutput(ref OutputBuffer, _kinematic);
            }

            //_kinematic.Actualize(Time.fixedDeltaTime, OutputBuffer);
            _kinematic.Actualize(OutputBuffer);
        }

        public int GetInstanceIdInScene()
        {
            return _instanceIdInScene;
        }

        //public void SetKinematicSteering(KinematicSteering kinematicSteering)
        //{
        //    _steering = kinematicSteering;
        //}

        //public void ApplyKinematicSteering()
        //{
        //    if (Behavior != _oldBehavior)
        //    {
        //        FreeOldSteering();
        //        ConfigureNewSteering();

        //        _oldBehavior = Behavior;
        //    }
        //}

        private void FreeOldSteering()
        {
            if (_steering != null)
            {
                SteeringTable.Instance.ReleaseBusySteering(_steering);
                _steering = null;
                Behavior  = EBehavior.None;
            }
        }

        //private void ConfigureNewSteering()
        //{
        //    switch (Behavior)
        //    {
        //        case EBehavior.None:
        //            _steering = null;
        //            break;

        //        case EBehavior.Seek:
        //            _steering = SteeringTable.Instance.GetSeekSteering(0, new StationaryLocation(transform.position));
        //            break;

        //        case EBehavior.Arrive:
        //            _steering = SteeringTable.Instance.GetArriveSteering(0, )
        //    }
        //}

        // useful for the CustomInspector
        public KinematicSteering GetSteering()
        {
            return _steering;
        }

        private KinematicSteering GetNewSteeringFromTable(EBehavior behavior)
        {
            FreeOldSteering();

            return SteeringTable.Instance.GetFreeSteering(behavior);
        }

        // ========================================================================
        // ||                      STEERING CONSTRUCTION                          ||
        // ========================================================================

        public void RemoveSteering()
        {
            FreeOldSteering();
        }

        public void AddSeekSteering(float maxSpeed, Location target)
        {
            KinematicSeek seek = (KinematicSeek) GetNewSteeringFromTable(EBehavior.Seek);

            seek.__KinematicSeek__(maxSpeed, target);

            _steering = seek;
        }

        public void AddArriveSteering(float maxSpeed, Location target,
            float timeToTarget, float targetRadius, float slowRadius)
        {
            KinematicArrive arrive = (KinematicArrive)GetNewSteeringFromTable(EBehavior.Arrive);

            arrive.__KinematicArrive__(maxSpeed, target, timeToTarget, targetRadius, slowRadius);

            _steering = arrive;
        }

        public void AddFleeSteering(float maxSpeed, Location target)
        {
            KinematicFlee flee = (KinematicFlee) GetNewSteeringFromTable(EBehavior.Flee);

            flee.__KinematicFlee(maxSpeed, target);

            _steering = flee;
        }
    }
}
