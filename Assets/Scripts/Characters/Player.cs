using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;
using UnityEngine;

namespace MonsterAdventure
{
    [RequireComponent(typeof(AnimatorComponent), typeof(Kinematic))]
    public class Player : MonoBehaviour
    {
        private bool _lastWasLeft;
        private bool _lastWasRight;
        private bool _lastWasFront;
        private bool _lastWasBack;

        private AnimatorComponent _animator;
        private Kinematic _kinematic;

        void Awake()
        {
            _animator = GetComponent<AnimatorComponent>();
            _kinematic = GetComponent<Kinematic>();

            _lastWasLeft = false;
            _lastWasRight = false;
            _lastWasFront = true;
            _lastWasBack = false;

            _animator.SetCurrentAnimation("idle", EOrientation.Face);
        }

        void FixedUpdate()
        {
            float horizontal = 0;
            float vertical = 0;

            horizontal = (int) Input.GetAxisRaw("Horizontal");
            vertical = (int) Input.GetAxisRaw("Vertical");

            bool isDiagonal = false;

            if (horizontal != 0 && vertical != 0)
            {
                horizontal /= 2;
                vertical /= 2;

                isDiagonal = true;
            }

            if (horizontal > 0 && !_lastWasRight && (!isDiagonal || _lastWasLeft))
            {
                _animator.SetCurrentAnimation("idle", EOrientation.Right);
                ResetBuffers();
                _lastWasRight = true;
            }
            else if (horizontal < 0 && !_lastWasLeft && (!isDiagonal || _lastWasRight))
            {
                _animator.SetCurrentAnimation("idle", EOrientation.Left);
                ResetBuffers();
                _lastWasLeft = true;
            }
            else if (vertical > 0 && !_lastWasBack && (!isDiagonal || _lastWasFront))
            {
                _animator.SetCurrentAnimation("idle", EOrientation.Back);
                ResetBuffers();
                _lastWasBack = true;
            }
            else if (vertical < 0 && !_lastWasFront && (!isDiagonal || _lastWasBack))
            {
                _animator.SetCurrentAnimation("idle", EOrientation.Face);
                ResetBuffers();
                _lastWasFront = true;
            }

            SteeringOutput output = new SteeringOutput();
            output.Linear = new Vector2(horizontal*1000, vertical*1000);
            _kinematic.Actualize(output, Time.fixedDeltaTime);
        }

        private void ResetBuffers()
        {
            _lastWasRight = false;
            _lastWasLeft = false;
            _lastWasBack = false;
            _lastWasFront = false;
        }
    }
}