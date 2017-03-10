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

        public List<InternalBehavior> BehaviorAndWeights;

        public InternalBehavior ActiveBehavior;

        private SteeringOutput _outputResult;
        private SteeringOutput _outputBuffer;

        private void Awake()
        {
            _outputResult = new SteeringOutput();
            _outputBuffer = new SteeringOutput();
        }

        public void SetActiveBehavior(EBehavior behavior, List<Kinematic> targets, float weight)
        {
            ActiveBehavior.Behavior = behavior;
            ActiveBehavior.Targets = targets;
            ActiveBehavior.Weight = weight;
        }

        public void Actualize(Kinematic character, float deltaTime)
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

            // do the activeBehavior
            FillOutputBuffer(ActiveBehavior, character);
            character.Actualize(_outputBuffer, deltaTime);

            // TODO : find a way to work with "instant orientation" and "is oriented" and the "result output buffer"

            _outputResult.Add(_outputBuffer);
            totalWeight += ActiveBehavior.Weight;

            // Divide the accumulated output by the total weight
            if (totalWeight > 0)
            {
                totalWeight = 1f/totalWeight;

                _outputResult.Scale(totalWeight);
            }


        }

        private void FillOutputBuffer(InternalBehavior internalBehavior, Kinematic character)
        {
            // clear the old values
            _outputBuffer.Reset();

            // Get the steering
            //behaviorAndWeight.Steering.ConfigureSteeringOutput(ref _outputBuffer, character);
            ApplySteering(internalBehavior.Behavior, character, internalBehavior.Targets);

            // scale it with the weight
            _outputBuffer.Scale(internalBehavior.Weight);
        }

        //private KinematicSteering AddSteering(EBehavior behavior, float weight = 1f)
        //{
        //    KinematicSteering steering = SteeringTable.Instance.GetFreeSteering(behavior);
        //    BehaviorAndWeights.Add(new BehaviorAndWeight(steering, weight));

        //    return steering;
        //}

        public SteeringOutput GetSteeringOutput()
        {
            return _outputResult;
        }

        private void ApplySteering(EBehavior behavior, Kinematic character, List<Kinematic> target)
        {
            _outputBuffer.Reset();

            Kinematic theClosest = null;

            switch (behavior)
            {
                case EBehavior.Seek:
                    theClosest = GetTheClosest(target, character);

                    if (theClosest != null)
                    {
                        Behavior.Seek(ref _outputBuffer, character, SteeringSpecs, theClosest.GetPosition());
                    }
                    break;

                case EBehavior.Flee:
                    theClosest = GetTheClosest(target, character);

                    if (theClosest != null)
                    {
                        Behavior.Flee(ref _outputBuffer, character, SteeringSpecs, theClosest.GetPosition());
                    }
                    break;

                case EBehavior.Arrive:
                    theClosest = GetTheClosest(target, character);

                    if (theClosest != null)
                    {
                        Behavior.Arrive(ref _outputBuffer, character, SteeringSpecs, theClosest.GetPosition());
                    }
                    break;

                case EBehavior.Face:
                    theClosest = GetTheClosest(target, character);

                    if (theClosest != null)
                    {
                        Behavior.Face(ref _outputBuffer, character, SteeringSpecs, theClosest.GetPosition());
                    }
                    break;

                case EBehavior.Pursue:
                    theClosest = GetTheClosest(target, character);

                    if (theClosest != null)
                    {
                        Behavior.Pursue(ref _outputBuffer, character, SteeringSpecs, theClosest);
                    }
                    break;

                case EBehavior.Evade:
                    theClosest = GetTheClosest(target, character);

                    if (theClosest != null)
                    {
                        Behavior.Evade(ref _outputBuffer, character, SteeringSpecs, theClosest);
                    }
                    break;

                case EBehavior.Wander:
                    Behavior.Wander(ref _outputBuffer, character, SteeringSpecs);
                    break;
            }
        }

        private Kinematic GetTheClosest(List<Kinematic> kinematics, Kinematic character)
        {
            float closestDistance = float.MaxValue;
            int bestIndex = -1;

            for (int i = 0; i < kinematics.Count; i++)
            {
                float currentDistance = Vector2.Distance(kinematics[i].GetPosition(), character.GetPosition());

                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    bestIndex = i;
                }
            }

            if (bestIndex == -1)
            {
                return null;
            }

            return kinematics[bestIndex];
        }
    }
}