using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    public class SteeringComponent : MonoBehaviour
    {
        public SteeringSpecs SteeringSpecs;

        public List<BehaviorAndWeight> BehaviorAndWeights;

        private SteeringOutput _outputResult;
        private SteeringOutput _outputBuffer;

        private void Awake()
        {
            _outputResult = new SteeringOutput();
            _outputBuffer = new SteeringOutput();
        }

        public void Actualize(Kinematic character)
        {
            float totalWeight = 0;
            _outputBuffer.Reset();
            _outputResult.Reset();

            for (int i = 0; i < BehaviorAndWeights.Count; i++)
            {
                // fill the output buffer with the steering scaled with the weight
                FillOutputBuffer(BehaviorAndWeights[i], character);

                // add the output to the result
                _outputResult.Add(_outputBuffer);

                // increase the total weight
                totalWeight += BehaviorAndWeights[i].Weight;
            }

            // Divide the accumulated output by the total weight
            if (totalWeight > 0)
            {
                totalWeight = 1f / totalWeight;

                _outputResult.Scale(totalWeight);
            }
        }

        private void FillOutputBuffer(BehaviorAndWeight behaviorAndWeight, Kinematic character)
        {
            // clear the old values
            _outputBuffer.Reset();

            // Get the steering
            behaviorAndWeight.Steering.ConfigureSteeringOutput(ref _outputBuffer, character);

            // scale it with the weight
            _outputBuffer.Scale(behaviorAndWeight.Weight);
        }

        public void AddActiveSteering(EBehavior behavior, List<Kinematic> targets)
        {
            // temporary
            BehaviorAndWeights.Clear();

            
        }

        private KinematicSteering AddSteering(EBehavior behavior, float weight = 1f)
        {
            KinematicSteering steering = SteeringTable.Instance.GetFreeSteering(behavior);
            BehaviorAndWeights.Add(new BehaviorAndWeight(steering, weight));

            return steering;
        }

        public SteeringOutput GetSteeringOutput()
        {
            return _outputResult;
        }

        // ========================================================================
        // ||                      STEERING CONSTRUCTION                          ||
        // ========================================================================

        //public void RemoveSteering()
        //{
        //    FreeOldSteering();
        //}

        public void AddSeekSteering(float maxSpeed, Location target)
        {
            KinematicSeek seek = (KinematicSeek)AddSteering(EBehavior.Seek);

            seek.__KinematicSeek__(maxSpeed, target);
        }

        public void AddArriveSteering(float maxSpeed, Location target,
            float timeToTarget, float targetRadius, float slowRadius)
        {
            KinematicArrive arrive = (KinematicArrive)AddSteering(EBehavior.Arrive);

            arrive.__KinematicArrive__(maxSpeed, target, timeToTarget, targetRadius, slowRadius);
        }

        public void AddFleeSteering(float maxSpeed, Location target)
        {
            KinematicFlee flee = (KinematicFlee)AddSteering(EBehavior.Flee);

            flee.__KinematicFlee__(maxSpeed, target);
        }

        public void AddWanderSteering(float maxSpeed, float maxRotation, float maxOffsetChange)
        {
            KinematicWander wander = (KinematicWander)AddSteering(EBehavior.Wander);

            wander.__KinematicWander__(maxSpeed, maxRotation, maxOffsetChange);
        }

        public void AddFaceSteering(Location target)
        {
            KinematicFace face = (KinematicFace)AddSteering(EBehavior.Face);

            face.__KinematicFace__(target);
        }

        public void AddPursueSteering(float maxSpeed, Kinematic target, float maxPredictionTime)
        {
            KinematicPursue pursue = (KinematicPursue)AddSteering(EBehavior.Pursue);

            pursue.__KinematicPursue__(maxSpeed, target, maxPredictionTime);
        }

        public void AddEvadeSteering(float maxSpeed, Kinematic target, float maxPredictionTime)
        {
            KinematicEvade evade = (KinematicEvade)AddSteering(EBehavior.Evade);

            evade.__KinematicEvade__(maxSpeed, target, maxPredictionTime);
        }

        public void AddDelegateWanderSteering(float maxSpeed, float wanderOffset,
            float wanderRadius, float wanderRate)
        {
            KinematicDelegateWander delegateWander =
                (KinematicDelegateWander)AddSteering(EBehavior.DelegateWander);

            delegateWander.__KinematicDelegateWander__(maxSpeed, wanderOffset, wanderRadius, wanderRate);
        }

        //public void AddCollisionAvoidance(float maxSpeed, float radius)
        //{
        //    _collisionAvoidance = GetNewCollisionAvoidance();

        //    _collisionAvoidance.__CollisionAvoidance__(maxSpeed, radius);
        //}
    }
}