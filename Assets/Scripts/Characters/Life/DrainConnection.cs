using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;
using UnityEngine;

namespace MonsterAdventure
{
    public class DrainConnection
    {
        public LifeComponent LifeComponent
        {
            get { return _lifeComponent; }
        }

        private LifeComponent _lifeComponent;
        private Transform _drainEnd;
        private float _bubbleSpeed;

        private float _timeBetweenBubble;
        private float _currentTime;

        public DrainConnection(LifeComponent lifeComponent, Transform drainEnd, float bubbleSpeed, float drainForce)
        {
            _lifeComponent = lifeComponent;
            _drainEnd = drainEnd;
            _bubbleSpeed = bubbleSpeed;
            
            _timeBetweenBubble = ComputeTimeBetweenBubble(drainForce);
            _currentTime = 0;
        }

        private float ComputeTimeBetweenBubble(float drainForce)
        {
            return drainForce/_lifeComponent.Resistance;
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
                    _drainEnd,
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
