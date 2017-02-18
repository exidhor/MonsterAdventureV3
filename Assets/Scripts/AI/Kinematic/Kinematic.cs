using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Kinematic : MonoBehaviour
    {
        private Rigidbody2D _rigidBody;

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
        }

        public void Actualize(float time, SteeringOutput steering)
        {
            Integrate(time, steering.Linear);
        }

        private void Integrate(float time, Vector2 velocity)
        {
            Move(velocity * time);
        }

        private void Move(Vector2 movement)
        {
            _rigidBody.MovePosition(_rigidBody.position + movement);
        }

        public Vector2 GetPosition()
        {
            return _rigidBody.position;
        }
    }
}
