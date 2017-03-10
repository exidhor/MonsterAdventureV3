using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DepthComponent : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private float _offset;

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _offset = ComputeOffset();
        }

        void LateUpdate()
        {
            if (_spriteRenderer.isVisible)
                _spriteRenderer.sortingOrder = (int) Camera.main.WorldToScreenPoint(_spriteRenderer.bounds.min).y*-1;
        }

        private float ComputeOffset()
        {
            Collider2D[] colliders = GetComponents<Collider2D>();

            float lowestY = float.MaxValue;
            Collider2D lowest = null;

            for (int i = 0; i < colliders.Length; i++)
            {
                float currentY = colliders[i].bounds.min.y;

                if (currentY < lowestY)
                {
                    lowestY = currentY;
                    lowest = colliders[i];
                }
            }

            if (lowest == null)
            {
                return 0;
            }

            return lowestY - _spriteRenderer.bounds.min.y;
        }
    }
}