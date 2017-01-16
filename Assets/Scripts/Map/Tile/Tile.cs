using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Tile : MonoBehaviour
    {
        private SpriteRenderer _spriteRender;

        private Coords _coords;

        private bool _isVisible;

        private void Awake()
        {
            _spriteRender = GetComponent<SpriteRenderer>();

            //SetIsVisible(false);
        }

        public void SetCoords(Coords coords)
        {
            _coords = coords;
        }

        public void SetSprite(Sprite sprite)
        {
           _spriteRender.sprite = sprite;
        }

        public void SetIsVisible(bool isVisible)
        {
            _isVisible = isVisible;

            gameObject.SetActive(_isVisible);
        }
    }
}
