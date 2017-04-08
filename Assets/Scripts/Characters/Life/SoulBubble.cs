using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;
using UnityEngine;

namespace MonsterAdventure
{
    [RequireComponent(typeof(Kinematic), typeof(SteeringComponent))]
    public class SoulBubble : MonoBehaviour
    {
        public float CarriedLife
        {
            get { return _carriedLife; }
        }

        private static readonly float lifeTime = 10f;
        private float _currentTime;

        private DrainComponent _target;

        private float _carriedLife;
        private bool _isDead;

        private Kinematic _kinematic;
        private SteeringComponent _steeringComponent;

        private PoolObject _poolObject;

        void Awake()
        {
            _steeringComponent = GetComponent<SteeringComponent>();
            _kinematic = GetComponent<Kinematic>();
        }

        public void Init(Vector2 start, DrainComponent drainComponent, float speed, float life, PoolObject poolObject)
        {
            _currentTime = 0;
            _isDead = false;

            transform.position = start;

            _target = drainComponent;

            _carriedLife = life;
            // set the scale in fonction of the carriedLife number
            transform.localScale =  new Vector2(_carriedLife, _carriedLife);

            _poolObject = poolObject;

            _kinematic.MaxSpeed = speed;

            _steeringComponent.ClearBehaviors();
            _steeringComponent.AddBehavior(EBehavior.Arrive, new TransformLocation(_target.transform), 1f);
        }

        void FixedUpdate()
        {
            float deltaTime = Time.fixedDeltaTime;

            if (!CheckForDeath(deltaTime))
            {
                if (!CheckForAbsorption())
                {
                    _steeringComponent.ActualizeSteering(_kinematic);
                    _steeringComponent.ApplySteeringOnKinematic(_kinematic, Time.fixedDeltaTime);
                }
            }
        }

        private bool CheckForDeath(float deltaTime)
        {
            if (_isDead)
                return true;

            _currentTime += deltaTime;

            if (_currentTime > lifeTime)
            {
                Die();
                return true;
            }

            return false;
        }

        private bool CheckForAbsorption()
        {
            float distance = Vector2.Distance(transform.position, _target.AbsorptionCenter.position);

            if (distance < _target.AbsorptionRadius)
            {
                // absorption
                _target.Absorb(this);
                Die();
                return true;
            }

            return false;
        }

        private void Die()
        {
            Debug.Log("Die");
            _isDead = true;
            DrainEngine.Instance.ReleaseSoulBubble(this);
        }

        public PoolObject GetPoolObject()
        {
            return _poolObject;
        }
    }
}