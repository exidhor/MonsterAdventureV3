using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ManaCore : MonoBehaviour
    {
        public float Speed;

        public float ShiningPeriod;
        public float ShiningSpeed;

        public float XFrequency;
        public float XAmplitude;

        public float YFrequency;
        public float YAmplitude;

        private SpriteRenderer _spriteRenderer;
        public SpriteRenderer LianaLight;
        public SpriteRenderer Light;

        private float _dephase;

        public float BasculeTime;
        public float CurrentTime;

        private Vector3 Direction;
        private float deltaAlpha = 1;

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            Direction = new Vector3(0, 1, 0);
        }

        void Update()
        {
            Color newColor = _spriteRenderer.color;

            CurrentTime += Time.deltaTime;

            if (CurrentTime > BasculeTime)
            {
                deltaAlpha *= -1;
                CurrentTime = 0;
            }

            //float deltaAlpha = Mathf.Sign((Time.time % ShiningPeriod * 2) - ShiningPeriod);

            newColor.a += deltaAlpha * ShiningSpeed* Time.deltaTime;

            if (newColor.a < 0)
            {
                newColor.a = 0;
            }
            else if (newColor.a > 1)
            {
                newColor.a = 1;
            }

            _spriteRenderer.color = newColor;
            LianaLight.color = newColor;
            Light.color = newColor;

            transform.position += XAmplitude * (Mathf.Sin(2 * Mathf.PI * XFrequency * Time.time + _dephase)
                                - Mathf.Sin(2 * Mathf.PI * XFrequency * (Time.time - Time.deltaTime) + _dephase)) * Direction;

            transform.position += YAmplitude * (Mathf.Sin(2 * Mathf.PI * YFrequency * Time.time + _dephase)
                                            - Mathf.Sin(2 * Mathf.PI * YFrequency * (Time.time - Time.deltaTime) + _dephase)) * transform.right;
        }
    }
}
