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
        public float OrientationInDegree;
        public float MaxSpeed;

        public float OrientationInRadian
        {
            get { return OrientationInDegree * Mathf.Deg2Rad; }
            set { OrientationInDegree = value * Mathf.Rad2Deg; }
        }

        public float SqrMaxSpeed
        {
            get { return MaxSpeed*MaxSpeed; }
        }

        private Rigidbody2D _rigidBody;

        [SerializeField]
        private float _rotationInDegree;

        private float _rotationInRadian
        {
            get { return _rotationInDegree * Mathf.Deg2Rad; }
            set { _rotationInDegree = value * Mathf.Rad2Deg; }
        }

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody2D>();

            OrientationInDegree = 0;
            _rotationInDegree = 0;
        }

        public void Actualize(SteeringOutput steering, float deltaTime)
        {
            ResetVelocity();

            if (steering.IsInstantOrientation)
            {
                OrientationInDegree = steering.AngularInDegree;
                _rotationInDegree = 0;
            }
            else
            {
                Rotate(steering.AngularInDegree, deltaTime);
            }

            Vector2 movement = steering.Linear;

            if (steering.IsOriented)
            {
                movement = ApplyRotation(movement);
            }

            Move(movement);

            CapVelocity();

            ActualizeOrientation();
        }

        private void ResetVelocity()
        {
            _rigidBody.velocity = Vector2.zero;
        }

        private void Rotate(float angularInDegree, float deltaTime)
        {
            OrientationInDegree += _rotationInDegree * deltaTime;
            OrientationInDegree %= 360f;

            _rotationInDegree += angularInDegree * deltaTime;
            //_rotationInDegree %= 360f;
        }

        private void CapVelocity()
        {
            Vector2 velocity = _rigidBody.velocity;

            if (velocity.sqrMagnitude > SqrMaxSpeed)
            {
                velocity.Normalize();
                velocity *= MaxSpeed;

                _rigidBody.velocity = velocity;
            }
        }

        private void Move(Vector2 velocity)
        {
            _rigidBody.velocity += velocity;
        }

        private Vector2 ApplyRotation(Vector2 movement)
        {
            return MathHelper.RotateVector(movement, OrientationInRadian);
        }

        public Vector2 GetPosition()
        {
            return _rigidBody.position;
        }

        public TransformLocation GetDynamicLocation()
        {
            return new TransformLocation(_rigidBody.transform);
        }

        public StationaryLocation GetInstantLocation()
        {
            return new StationaryLocation(GetPosition());
        }

        public Vector2 GetVelocity()
        {
            return _rigidBody.velocity;
        }

        private void ActualizeOrientation()
        {
            // verify if it need actualization, otherwise
            // we keep the old value
            if (GetVelocity().sqrMagnitude > float.Epsilon * float.Epsilon)
            {
                OrientationInRadian = Mathf.Atan2(GetVelocity().y, GetVelocity().x);
            }
        }

        public Vector2 GetOrientationAsVector()
        {
            return MathHelper.GetDirectionFromAngle(OrientationInRadian);
        }

        void OnDrawGizmosSelected()
        {
            if(_rigidBody == null)
                return;

            Gizmos.color = Color.magenta;

            Vector2 direction = GetOrientationAsVector();

            Vector2 first = GetPosition();
            Vector2 second = first + direction;

            Gizmos.DrawLine(first, second);
        }

        public override string ToString()
        {
            return _rigidBody.name;
        }
    }
}