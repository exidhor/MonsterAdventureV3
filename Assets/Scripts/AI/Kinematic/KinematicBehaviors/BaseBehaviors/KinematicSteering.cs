using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    public abstract class KinematicSteering : MonoBehaviour
    {
        private PoolObject _poolObject;
        private bool _isInit = false;

        private float _maxSpeed;

        public abstract void GiveSteering(ref SteeringOutput output, Kinematic character);

        public void ConfigureSteeringOutput(ref SteeringOutput output, Kinematic character)
        {
            if (_isInit)
            {
                GiveSteering(ref output, character);
            }
            else
            {
                output.Reset();
            }
        }

        protected void __KinematicSteering__(float maxSpeed)
        {
            _maxSpeed = maxSpeed;

            _isInit = true;
        }

        public void SetMaxSpeed(float maxSpeed)
        {
            _maxSpeed = maxSpeed;
        }

        public float GetMaxSpeed()
        {
            return _maxSpeed;
        }

        public void SetPoolObject(PoolObject poolObject)
        {
            _poolObject = poolObject;
        }

        public PoolObject GetPoolObject()
        {
            return _poolObject;
        }


    }
}
