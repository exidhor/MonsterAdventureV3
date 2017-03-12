using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    [Serializable]
    public class Animation
    {
        public List<Sprite> Sprites;
        public float Speed;
        public bool Flip;
        public bool Loop;
        public string Tag;

        private float _currentTime;
        private int _currentIndex;

        public void Reset()
        {
            _currentIndex = 0;
            _currentTime = 0;
        }

        public void Actualize(float deltaTime, SpriteRenderer spriteRenderer, Transform objectTransform)
        {
            _currentTime += deltaTime;

            while (_currentTime > Speed)
            {
                _currentTime -= Speed;
                _currentIndex++;

                if (_currentIndex > Sprites.Count - 1)
                {
                    if (Loop)
                    {
                        _currentIndex = 0;
                    }
                    else
                    {
                        _currentIndex = Sprites.Count - 1;
                    }
                }
            }

            if (Flip)
            {
                // check if the transform is already flipped
                if (objectTransform.localScale.x > 0)
                {
                    objectTransform.localScale = new Vector3(-objectTransform.localScale.x,
                        objectTransform.localScale.y,
                        objectTransform.localScale.z);
                }
            }
            else
            {
                if (objectTransform.localScale.x < 0)
                {
                    objectTransform.localScale = new Vector3(-objectTransform.localScale.x,
                        objectTransform.localScale.y,
                        objectTransform.localScale.z);
                }
            }

            // set the sprite to the renderer
            spriteRenderer.sprite = Sprites[_currentIndex];
        }
    }
}