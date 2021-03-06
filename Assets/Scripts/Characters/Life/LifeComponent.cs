﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class LifeComponent : MonoBehaviour
    {
        public LifeData LifeData;

        public Transform AbsorptionPoint;

        private float LifeLevelOffset
        {
            get { return LifeData.Regeneration; }
        }

        //public bool IsAlive
        //{
        //    get { return LifeData.Life > float.Epsilon; }
        //}

        public bool IsAlive
        {
            get { return LifeData.IsAlive; }
        }

        private AnimatorComponent _animatorComponent;

        private float _timeBetweenLifeSample = 0.1f;
        private float _currentTimeForBuffer;
        private float _oldLifeBuffer;
        private List<int> _lifeLevel;
        private int _currentLifeLevel;

        void Awake()
        {
            _animatorComponent = GetComponent<AnimatorComponent>();
            _oldLifeBuffer = LifeData.Life;
        }

        void Start()
        {
            if (_animatorComponent != null)
                ConstructLifeLevel();
        }

        public float ExtractSoulBubble()
        {
            AddLife(-LifeData.LifePerBubble);

            return LifeData.LifePerBubble;
        }

        private void ConstructLifeLevel()
        {
            // temporary ?
            _lifeLevel = _animatorComponent.GetAllNumberFromTag("life");

            _lifeLevel.Sort((x, y) => x.CompareTo(y));

            // set the life level index
            _currentLifeLevel = FindLifeLevelIndex(LifeData.Life);
            _currentTimeForBuffer = 0f;
        }

        /// <summary>
        /// return immediate greater index
        /// </summary>
        /// <param name="life"></param>
        /// <returns></returns>
        private int FindLifeLevelIndex(float life, float offset = 0f, bool reverseIteration = false)
        {
            int result = 0;

            if (reverseIteration)
            {
                result = _currentLifeLevel;
                int i = result - 1;

                while (i >= 0 && life < _lifeLevel[i] - offset)
                {
                    result = i;
                    i--;
                }
            }
            else
            {
                result = _currentLifeLevel;
                int i = result + 1;

                while (i < _lifeLevel.Count && life > _lifeLevel[i] + offset)
                {
                    result = i;
                    i++;
                }
            }

            return result;
        }

        private int FindLifeLevelIndexWithOffset(float oldLife, float newLife)
        {
            if (oldLife < newLife)
            {
                return FindLifeLevelIndex(newLife, LifeLevelOffset, false);
            }
            else if (oldLife > newLife)
            {
                return FindLifeLevelIndex(newLife, LifeLevelOffset, true);
            }

            return _currentLifeLevel;
        }

        void Update()
        {
            float deltaTime = Time.deltaTime;

            ActualizeLife(deltaTime);

            ActualizeOldLifeBuffer(deltaTime);
        }

        private void ActualizeOldLifeBuffer(float deltaTime)
        {
            _currentTimeForBuffer += deltaTime;

            while (_currentTimeForBuffer > _timeBetweenLifeSample)
            {
                _currentTimeForBuffer -= _timeBetweenLifeSample;
                _oldLifeBuffer = LifeData.Life;
            }
        }

        private void ActualizeLife(float deltaTime)
        {
            //NaturalRegeneration(deltaTime);
            LifeData.Regenerate(deltaTime);

            // verify the life level
            if (_animatorComponent != null)
                CheckForLifeLevel();
        }

        //private void NaturalRegeneration(float deltaTime)
        //{
        //    if (LifeData.IsAlive)
        //        AddLife(LifeData.Regeneration * deltaTime);
        //}

        private void CheckForLifeLevel()
        {
            if (!LifeData.IsAlive)
            {
                _animatorComponent.SetCurrentAnimation("death");
                return;
            }

            int newLifeLevel = FindLifeLevelIndexWithOffset(_oldLifeBuffer, LifeData.Life);

            if (newLifeLevel != -1 && newLifeLevel != _currentLifeLevel)
            {
                Debug.Log("Change life level (old : " + _currentLifeLevel + ", new : " + newLifeLevel + ")");

                _currentLifeLevel = newLifeLevel;
                _animatorComponent.SetCurrentAnimation("life", _lifeLevel[_currentLifeLevel]);
            }
        }

        private void AddLife(float toAdd)
        {
            LifeData.Life += toAdd;
        }

        //private void SetLife(float newLife)
        //{
        //    if (newLife < 0)
        //    {
        //        Life = 0;
        //    }
        //    else if (newLife > MaxLife)
        //    {
        //        Life = MaxLife;
        //    }
        //    else
        //    {
        //        Life = newLife;
        //    }
        //}
    }
}