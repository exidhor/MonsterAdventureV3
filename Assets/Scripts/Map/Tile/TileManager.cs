using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class TileManager : MonoBehaviour
    {
        public Tile tilePrefab;

        private Tile[,] _tiles;

        private SectorManager _sectorManager;

        private RandomGenerator _random;

        private void Awake()
        {
            // nothing
        }

        public void Construct(FakeDoubleEntryList<Sector> lastList, 
            int tileSize, 
            RandomGenerator random, 
            SectorManager sectorManager)
        {
            _random = random;

            _sectorManager = sectorManager;

            float offset = (lastList.lineSize*tileSize - tileSize)/2f;

            //_tiles = new Tile[lastList.lineSize, lastList.lineSize];

            for (int i = 0; i < lastList.lineSize; i++)
            {
                for (int j = 0; j < lastList.lineSize; j++)
                {
                    Tile newTile = InstantiateTile(tilePrefab, j, i);

                    Vector2 tilePosition;
                    tilePosition.x = j * tileSize - offset;
                    tilePosition.y = i * tileSize - offset;

                    newTile.transform.position = tilePosition;

                    _sectorManager.Get(j, i, _sectorManager.resolution).SetTile(newTile);
                }
            }
            /*
            for (int i = 0; i < _tiles.GetLength(0); i++)
            {
                for (int j = 0; j < _tiles.GetLength(1); j++)
                {
                    Tile newTile = InstantiateTile(tilePrefab, i, j);

                    Vector2 tilePosition;
                    tilePosition.x = i * tileSize - offset;
                    tilePosition.y = j * tileSize - offset;

                    newTile.transform.position = tilePosition;

                    _sectorManager.Get(i, j, _sectorManager.resolution - 1).SetTile(newTile);
                    //_tiles[i, j] = newTile;
                }
            }*/
        }

        private Tile InstantiateTile(Tile prefab, int x, int y)
        {
            Tile tile = Instantiate<Tile>(prefab);
            tile.transform.parent = gameObject.transform;
            tile.name = prefab.name + " (" + x + ", " + y + ")";
            tile.SetCoords(new Coords(x, y));

            return tile;
        }

        public void Generate(FakeDoubleEntryList<Sector> lastList)
        {
            ApplyBiome(lastList);
        }

        private void ApplyBiome(FakeDoubleEntryList<Sector> lastList)
        {
            for(int i = 0; i < lastList.lineSize; i++)
            {
                for (int j = 0; j < lastList.lineSize; j++)
                {
                    Sprite[] biomeSprite = lastList.GetElement(i, j).GetBiomeList().GetBiomeContainers()[0].biome.data.sprites;

                    int spriteIndex = _random.Next(biomeSprite.Length);

                    lastList.GetElement(i,j).GetTile().SetSprite(biomeSprite[spriteIndex]);
                }
            }
            /*
            for (int i = 0; i < _tiles.GetLength(0); i++)
            {
                for (int j = 0; j < _tiles.GetLength(1); j++)
                {
                    Sprite[] biomeSprite = lastList.GetElement(i, j).GetBiomeList().GetBiomeContainers()[0].biome.data.sprites;

                    int spriteIndex = _random.Next(biomeSprite.Length);

                    _tiles[i,j].SetSprite(biomeSprite[spriteIndex]);
                }
            }*/
        }
    }
}
