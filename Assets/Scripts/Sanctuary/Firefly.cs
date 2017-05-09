using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Firefly : MonoBehaviour
    {
        public float Speed;
        public float MaxLifeTime;
        public float LifeTime;

        public bool IsDying;
        public bool IsDead;

        public float DeathSpeed;

        public float ShiningPeriod;
        public float ShiningSpeed;

        public float Frequency;
        public float Amplitude;

        private SpriteRenderer _spriteRenderer;

        private float _dephase;

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetUp(float speed, float shiningSpeed, float scale, float dephase)
        {
            Speed = speed;
            ShiningSpeed = shiningSpeed;
            LifeTime = 0;
            _dephase = dephase;

            transform.localScale = new Vector3(scale, scale, scale);

            IsDying = false;
            IsDead = false;
        }

        void Update()
        {
            if (!IsDying)
            {
                LifeTime += Time.deltaTime;

                if (LifeTime > MaxLifeTime)
                {
                    IsDying = true;
                }
                else
                {
                    Color newColor = _spriteRenderer.color;

                    float deltaAlpha = (Time.time % ShiningPeriod*2) - ShiningPeriod;

                    newColor.a += deltaAlpha  * ShiningSpeed;

                    _spriteRenderer.color = newColor;
                }
            }
            else
            {
                Color newColor = _spriteRenderer.color;

                newColor.a -= DeathSpeed*Time.deltaTime;

                if (newColor.a <= 0f)
                {
                    Destroy(gameObject);
                }
                else
                {
                    _spriteRenderer.color = newColor;
                }
            }

            transform.position += Amplitude*(Mathf.Sin(2*Mathf.PI*Frequency*Time.time + _dephase)
                                - Mathf.Sin(2*Mathf.PI*Frequency*(Time.time - Time.deltaTime) + _dephase))*transform.right;

            transform.position += new Vector3(0, Time.deltaTime * Speed);
        }
    }
}
