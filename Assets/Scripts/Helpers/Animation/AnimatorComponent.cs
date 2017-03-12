using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class AnimatorComponent : MonoBehaviour
    {
        public int CurrentIndex;
        public List<Animation> Animations;

        private Dictionary<string, int> _animationIndex;

        private SpriteRenderer _spriteRenderer;

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            ConstructDictionary();
        }

        private void ConstructDictionary()
        {
            _animationIndex = new Dictionary<string, int>();

            for (int i = 0; i < Animations.Count; i++)
            {
                _animationIndex.Add(Animations[i].Tag, i);
            }
        }

        void LateUpdate()
        {
            Animations[CurrentIndex].Actualize(Time.deltaTime, _spriteRenderer, transform);
        }

        public void SetCurrentAnimation(string tag)
        {
            CurrentIndex = _animationIndex[tag];
            Animations[CurrentIndex].Reset();
        }
    }
}
