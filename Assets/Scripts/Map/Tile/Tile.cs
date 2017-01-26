using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class Tile : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;

        private Coords _coordsInSector;

        private void Awake()
        {

        }

        public void Construct(Coords coordsInSector, Vector2 position)
        {
            _coordsInSector = coordsInSector;

            transform.position = position;
        }
    }
}
