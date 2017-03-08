using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    [Serializable]
    public class SteeringSpecs
    {
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _minTimeToTarget;
        [SerializeField] private float _radiusMarginError;
        [SerializeField] private float _slowArriveRadius;
        [SerializeField] private float _maxPredictionTime;
        [SerializeField] private float _maxWanderOffset;
        [SerializeField] private float _wanderRadius;
        [SerializeField] private float _wanderRate;

        public float MaxSpeed
        {
            get { return _maxSpeed; }
        }

        public float MinTimeToTarget
        {
            get { return _minTimeToTarget; }
        }

        public float RadiusMarginError
        {
            get { return _radiusMarginError; }
        }

        public float SlowArriveRadius
        {
            get { return _slowArriveRadius; }
        }

        public float MaxPredictionTime
        {
            get { return _maxPredictionTime; }
        }

        public float MaxWanderOffset
        {
            get { return _maxWanderOffset; }
        }

        public float WanderRadius
        {
            get { return _wanderRadius; }
        }

        public float WanderRate
        {
            get { return _wanderRate; }
        }
    }
}