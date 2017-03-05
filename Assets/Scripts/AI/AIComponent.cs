using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.LightEffect;
using UnityEngine;

namespace MonsterAdventure.AI
{
    //[RequireComponent(typeof(Kinematic))]
    public class AIComponent : MonoBehaviour
    {
        public EBehavior Behavior = EBehavior.None;
        private EBehavior _oldBehavior = EBehavior.None;   // todo : check if it is used 
        public SteeringOutput OutputBuffer;

        private Kinematic _kinematic;

        private KinematicSteering _steering;

        // tmp
        private CollisionAvoidance _collisionAvoidance;
        private List<Kinematic> _avoidanceTargets = new List<Kinematic>();

        private int _instanceIdInScene;

        private void Awake()
        {
            _instanceIdInScene = GetInstanceID();
        }

        private void OnEnable()
        {
            AIEngine.Instance.Register(this);
        }

        private void OnDisable()
        {
            if (AIEngine.InternalInstance != null)
            {
                AIEngine.Instance.Unregister(this);
            }
        }

        private void Start()
        {
            _kinematic = GetComponent<Kinematic>();

            OutputBuffer = new SteeringOutput();

            _steering = null;

            AddCollisionAvoidance(3, 1);
        }

        private void Update()
        {

        }

        private void FixedUpdate()
        {

        }

        public void ActualizeSteerings()
        {
            // begin with collision avoidance
            OutputBuffer.Reset();

            if (_collisionAvoidance != null)
            {
                _collisionAvoidance.ConfigureSteeringOutput(ref OutputBuffer, _kinematic);
            }

            if (_steering != null && !OutputBuffer.IsFilled())
            {
                OutputBuffer.Reset();

                _steering.ConfigureSteeringOutput(ref OutputBuffer, _kinematic);
            }
        }

        public void ActualizeKinematic(float deltaTime)
        {
            _kinematic.Actualize(OutputBuffer, deltaTime);
        }

        public int GetInstanceIdInScene()
        {
            return _instanceIdInScene;
        }

        private void FreeOldSteering()
        {
            if (_steering != null)
            {
                SteeringTable.Instance.ReleaseBusySteering(_steering);
            }

            _steering = null;
            Behavior = EBehavior.None;
        }

        // useful for the CustomInspector
        public KinematicSteering GetSteering()
        {
            return _steering;
        }

        private CollisionAvoidance GetNewCollisionAvoidance()
        {
            if (_collisionAvoidance != null)
            {
                SteeringTable.Instance.ReleaseBusySteering(_collisionAvoidance);
                _collisionAvoidance = null;
            }

            return (CollisionAvoidance) SteeringTable.Instance.GetFreeSteering(EBehavior.CollisionAvoidance);
        }

        private KinematicSteering GetNewSteeringFromTable(EBehavior behavior)
        {
            FreeOldSteering();

            Behavior = behavior;

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

            flee.__KinematicFlee__(maxSpeed, target);

            _steering = flee;
        }

        public void AddWanderSteering(float maxSpeed, float maxRotation, float maxOffsetChange)
        {
            KinematicWander wander = (KinematicWander) GetNewSteeringFromTable(EBehavior.Wander);

            wander.__KinematicWander__(maxSpeed, maxRotation, maxOffsetChange);

            _steering = wander;
        }

        public void AddFaceSteering(Location target)
        {
            KinematicFace face = (KinematicFace) GetNewSteeringFromTable(EBehavior.Face);

            face.__KinematicFace__(target);

            _steering = face;
        }

        public void AddPursueSteering(float maxSpeed, Kinematic target, float maxPredictionTime)
        {
            KinematicPursue pursue = (KinematicPursue) GetNewSteeringFromTable(EBehavior.Pursue);

            pursue.__KinematicPursue__(maxSpeed, target, maxPredictionTime);

            _steering = pursue;
        }

        public void AddEvadeSteering(float maxSpeed, Kinematic target, float maxPredictionTime)
        {
            KinematicEvade evade = (KinematicEvade) GetNewSteeringFromTable(EBehavior.Evade);

            evade.__KinematicEvade__(maxSpeed, target, maxPredictionTime);

            _steering = evade;
        }

        public void AddDelegateWanderSteering(float maxSpeed, float wanderOffset,
            float wanderRadius, float wanderRate)
        {
            KinematicDelegateWander delegateWander =
                (KinematicDelegateWander) GetNewSteeringFromTable(EBehavior.DelegateWander);

            delegateWander.__KinematicDelegateWander__(maxSpeed, wanderOffset, wanderRadius, wanderRate);

            _steering = delegateWander;
        }

        public void AddCollisionAvoidance(float maxSpeed, float radius)
        {
            _collisionAvoidance = GetNewCollisionAvoidance();

            _collisionAvoidance.__CollisionAvoidance__(maxSpeed, radius);
        }
    }
}
