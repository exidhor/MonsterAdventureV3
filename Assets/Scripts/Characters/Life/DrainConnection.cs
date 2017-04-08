using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;
using UnityEngine;

namespace MonsterAdventure
{
    [Serializable]
    public class DrainConnection
    {
        private static readonly float minTimeBetweenBubble = 0.1f;

        public LifeComponent LifeComponent
        {
            get { return _lifeComponent; }
        }

        private LifeComponent _lifeComponent;
        private DrainComponent _target;
        private float _bubbleSpeed;

        private float _timeBetweenBubble;
        private float _currentTime;

        public DrainConnection(LifeComponent lifeComponent, DrainComponent target, float bubbleSpeed, float drainForce)
        {
            _lifeComponent = lifeComponent;
            _target = target;
            _bubbleSpeed = bubbleSpeed;
            
            _timeBetweenBubble = ComputeTimeBetweenBubble(drainForce);
            _currentTime = 0;
        }

        private float ComputeTimeBetweenBubble(float drainForce)
        {
            if (_lifeComponent.LifeData.Resistance == 0f)
                return minTimeBetweenBubble;

            float timeBetweenBubble = _lifeComponent.LifeData.Resistance / drainForce;

            if (timeBetweenBubble < minTimeBetweenBubble)
                return minTimeBetweenBubble;

            return timeBetweenBubble;
        }

        public void Actualize(float deltaTime)
        {
            _currentTime += deltaTime;
        }

        public bool IsReadyToSpawn()
        {
            return _currentTime >= _timeBetweenBubble;
        }

        public bool TargetIsAlive()
        {
            return _lifeComponent.IsAlive;
        }

        public void Spawn()
        {
            if (IsReadyToSpawn())
            {
                _currentTime -= _timeBetweenBubble;

                DrainEngine.Instance.ConstructSoulBubble(_lifeComponent.AbsorptionPoint.position, 
                    _target,
                    _bubbleSpeed,
                    _lifeComponent.ExtractSoulBubble());
            }
            else
            {
                Debug.LogWarning("Trying to spwan a not ready connection");
            }
        }
    }
}
