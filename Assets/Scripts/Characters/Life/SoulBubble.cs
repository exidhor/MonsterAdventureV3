using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;
using UnityEngine;

namespace MonsterAdventure
{
    [RequireComponent(typeof(SteeringComponent))]
    public class SoulBubble : MonoBehaviour
    {
        private Location _destination;
        private float _speed;
        private float _life;

        private SteeringComponent _steeringComponent;

        private PoolObject _poolObject;

        void Awake()
        {
            _steeringComponent = GetComponent<SteeringComponent>();
        }

        public void Init(Vector2 start, Location destination, float speed, float life, PoolObject poolObject)
        {
            transform.position = start;

            _destination = destination;
            _speed = speed;
            _life = life;
            _poolObject = poolObject;
        }

        void FixedUpdate()
        {
            
        }
    }
}
