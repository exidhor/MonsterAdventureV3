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

        public List<InternalBehavior> InternalBehaviors;

        public InternalBehavior ActiveBehavior;

        private List<WeightedSteeringOutput> _weightedOutputs;

        //private SteeringOutput _outputResult;
        //private SteeringOutput _outputBuffer;

        private void Awake()
        {
            //_outputResult = new SteeringOutput();
            //_outputBuffer = new SteeringOutput();

            _weightedOutputs = new List<WeightedSteeringOutput>();
        }

        public void SetActiveBehavior(EBehavior behavior, List<Kinematic> targets, float weight)
        {
            ActiveBehavior.Behavior = behavior;
            ActiveBehavior.Targets = targets;
            ActiveBehavior.Weight = weight;
        }

        public void ActualizeSteering(Kinematic character)
        {
            SteeringOutput outputBuffer;

            float totalWeight = 0;
            //_outputBuffer.Reset();
            //_outputResult.Reset();
            _weightedOutputs.Clear();

            for (int i = 0; i < InternalBehaviors.Count; i++)
            {
                // fill the output buffer with the steering
                FillOutput(InternalBehaviors[i], character, out outputBuffer);

                // add the output to the result
                _weightedOutputs.Add(new WeightedSteeringOutput(outputBuffer, InternalBehaviors[i].Weight));

                // increase the total weight
                totalWeight += InternalBehaviors[i].Weight;
            }

            // do the activeBehavior
            FillOutput(ActiveBehavior, character, out outputBuffer);

            _weightedOutputs.Add(new WeightedSteeringOutput(outputBuffer, ActiveBehavior.Weight));

            //_outputResult.Add(_outputBuffer);
            totalWeight += ActiveBehavior.Weight;

            // Divide the accumulated output by the total weight
            if (totalWeight > 0)
            {
                //totalWeight = 1f/totalWeight;

                //_outputResult.Scale(totalWeight);

                for (int i = 0; i < _weightedOutputs.Count; i++)
                {
                    _weightedOutputs[i].Scale(totalWeight);
                }
            }
        }

        public void ApplySteeringOnKinematic(Kinematic kinematic, float deltaTime)
        {
            for (int i = 0; i < _weightedOutputs.Count; i++)
            {
                kinematic.Actualize(_weightedOutputs[i].Output, deltaTime);
            }
        }

        private void FillOutput(InternalBehavior internalBehavior, Kinematic character, out SteeringOutput toFill)
        {
            toFill = new SteeringOutput();

            // Get the steering
            //behaviorAndWeight.Steering.ConfigureSteeringOutput(ref _outputBuffer, character);
            GiveSteering(internalBehavior.Behavior, character, internalBehavior.Targets, ref toFill);

            // scale it with the weight
            //_outputBuffer.Scale(internalBehavior.Weight);
        }

        //private KinematicSteering AddSteering(EBehavior behavior, float weight = 1f)
        //{
        //    KinematicSteering steering = SteeringTable.Instance.GetFreeSteering(behavior);
        //    BehaviorAndWeights.Add(new BehaviorAndWeight(steering, weight));

        //    return steering;
        //}

        //public SteeringOutput GetSteeringOutput()
        //{
        //    return _outputResult;
        //}

        private void GiveSteering(EBehavior behavior, Kinematic character, List<Kinematic> target, ref SteeringOutput toFill)
        {
            Kinematic theClosest = null;

            switch (behavior)
            {
                case EBehavior.Seek:
                    theClosest = GetTheClosest(target, character);

                    if (theClosest != null)
                    {
                        Behavior.Seek(ref toFill, character, SteeringSpecs, theClosest.GetPosition());
                    }
                    break;

                case EBehavior.Flee:
                    theClosest = GetTheClosest(target, character);

                    if (theClosest != null)
                    {
                        Behavior.Flee(ref toFill, character, SteeringSpecs, theClosest.GetPosition());
                    }
                    break;

                case EBehavior.Arrive:
                    theClosest = GetTheClosest(target, character);

                    if (theClosest != null)
                    {
                        Behavior.Arrive(ref toFill, character, SteeringSpecs, theClosest.GetPosition());
                    }
                    break;

                case EBehavior.Face:
                    theClosest = GetTheClosest(target, character);

                    if (theClosest != null)
                    {
                        Behavior.Face(ref toFill, character, SteeringSpecs, theClosest.GetPosition());
                    }
                    break;

                case EBehavior.Pursue:
                    theClosest = GetTheClosest(target, character);

                    if (theClosest != null)
                    {
                        Behavior.Pursue(ref toFill, character, SteeringSpecs, theClosest);
                    }
                    break;

                case EBehavior.Evade:
                    theClosest = GetTheClosest(target, character);

                    if (theClosest != null)
                    {
                        Behavior.Evade(ref toFill, character, SteeringSpecs, theClosest);
                    }
                    break;

                case EBehavior.Wander:
                    Behavior.Wander(ref toFill, character, SteeringSpecs);
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