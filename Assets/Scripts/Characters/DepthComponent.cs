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
        private Vector3 _offset;

        void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

            _offset = ComputeOffset();
        }

        void LateUpdate()
        {
            if (_spriteRenderer.isVisible)
                _spriteRenderer.sortingOrder = GetSortingOrder();
        }

        private Vector2 ComputeOffset()
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
                return Vector2.zero;
            }

            return new Vector2(0, lowestY - _spriteRenderer.bounds.min.y);
        }

        private int GetSortingOrder()
        {
            return (int) Camera.main.WorldToScreenPoint(_spriteRenderer.bounds.min + _offset).y*-1;
        }
    }
}