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

        private List<WeightedSteeringOutput> _weightedOutputs;

        private void Awake()
        {
            _weightedOutputs = new List<WeightedSteeringOutput>();
        }

        public void ClearBehaviors()
        {
            InternalBehaviors.Clear();
        }

        public void AddBehavior(EBehavior behavior, List<Presence> targets, float weight)
        {
            InternalBehaviors.Add(new InternalBehavior(behavior, targets, weight));
        }

        public void ActualizeSteering(Kinematic character)
        {
            SteeringOutput outputBuffer;

            float totalWeight = 0;
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

            // Divide the accumulated output by the total weight
            if (totalWeight > 0)
            {
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
            GiveSteering(internalBehavior.Behavior, character, internalBehavior.Targets, ref toFill);
        }

        private void GiveSteering(EBehavior behavior, Kinematic character, List<Presence> target, ref SteeringOutput toFill)
        {
            Presence theClosest = null;

            switch (behavior)
            {
                case EBehavior.Seek:
                    theClosest = GetTheClosest(target, character);

                    if (theClosest != null)
                    {
                        Behavior.Seek(ref toFill, character, SteeringSpecs, theClosest.Position);
                    }
                    break;

                case EBehavior.Flee:
                    theClosest = GetTheClosest(target, character);

                    if (theClosest != null)
                    {
                        Behavior.Flee(ref toFill, character, SteeringSpecs, theClosest.Position);
                    }
                    break;

                case EBehavior.Arrive:
                    theClosest = GetTheClosest(target, character);

                    if (theClosest != null)
                    {
                        Behavior.Arrive(ref toFill, character, SteeringSpecs, theClosest.Position);
                    }
                    break;

                case EBehavior.Face:
                    theClosest = GetTheClosest(target, character);

                    if (theClosest != null)
                    {
                        Behavior.Face(ref toFill, character, SteeringSpecs, theClosest.Position);
                    }
                    break;

                case EBehavior.Pursue:
                    theClosest = GetTheClosest(target, character);

                    if (theClosest == null)
                        break;

                    if (theClosest.Kinematic == null)
                    {
                        Behavior.Arrive(ref toFill, character, SteeringSpecs, theClosest.Position);
                    }
                    else
                    {
                        Behavior.Pursue(ref toFill, character, SteeringSpecs, theClosest.Kinematic);
                    }
                    break;

                case EBehavior.Evade:
                    theClosest = GetTheClosest(target, character);

                    if (theClosest == null)
                        break;

                    if (theClosest.Kinematic == null)
                    {
                        Behavior.Flee(ref toFill, character, SteeringSpecs, theClosest.Position);
                    }
                    else
                    {
                        Behavior.Evade(ref toFill, character, SteeringSpecs, theClosest.Kinematic);
                    }
                    break;

                case EBehavior.Wander:
                    Behavior.Wander(ref toFill, character, SteeringSpecs);
                    break;
            }
        }

        private Presence GetTheClosest(List<Presence> presences, Kinematic character)
        {
            float closestDistance = float.MaxValue;
            int bestIndex = -1;

            for (int i = 0; i < presences.Count; i++)
            {
                float currentDistance = Vector2.Distance(presences[i].Position, character.GetPosition());

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

            return presences[bestIndex];
        }
    }
}