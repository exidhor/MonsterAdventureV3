using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    public class SteeringComponent : MonoBehaviour
    {
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

        public SteeringOutput GetSteeringOutput()
        {
            return _outputResult;
        }
    }
}