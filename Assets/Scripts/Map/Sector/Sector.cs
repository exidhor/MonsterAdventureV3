using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.VR;

namespace MonsterAdventure
{
    public class Sector : MonoBehaviour
    {
        private Rect _bounds;

        private Coords _coords;

        private Tile[,] _tiles;

        public void Construct(Rect bounds, Coords coords)
        {
            _bounds = bounds;
            transform.position = _bounds.position;

            _coords = coords;

            name = "Sector (" + coords.abs + ", " + coords.ord + ")";
        }

        public void ConstructTile(uint tileCount, Tile tilePrefab, float tileSize)
        {
            _tiles = new Tile[tileCount, tileCount];

            for (int i = 0; i < _tiles.GetLength(0); i++)
            {
                for (int j = 0; j < _tiles.GetLength(1); j++)
                {
                    _tiles[i, j] = InstantiateTile(new Coords(i, j), tilePrefab, tileSize);
                }
            }
        }

        private Tile InstantiateTile(Coords coordsInSector, Tile tilePrefab, float tileSize)
        {
            Tile tile = Instantiate<Tile>(tilePrefab);
            tile.transform.parent = gameObject.transform;

            Vector2 position = ComputeTilePosition(coordsInSector, tileSize);

            tile.Construct(coordsInSector, position);

            return tile;
        }

        private Vector2 ComputeTilePosition(Coords coordsInSector, float tileSize)
        {
            Vector2 position = new Vector2(transform.position.x, transform.position.y);

            position.x += coordsInSector.abs*tileSize;
            position.y += coordsInSector.ord*tileSize;

            return position;
        }
    }
}
