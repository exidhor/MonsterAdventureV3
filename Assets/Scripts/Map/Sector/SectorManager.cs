using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class SectorManager : MonoBehaviour
    {
        public uint Count
        {
            get { return (uint)(_sectors.GetLength(0)*_sectors.GetLength(1)); }
        }

        public uint LineSize
        {
            get { return (uint) (_sectors.GetLength(0)); }
        }

        public float SectorSize
        {
            get { return _sectorSize; }
        }

        private bool _isInitialized;

        private Sector[,] _sectors;
        private float _sectorSize;
        private uint _splitLevel;

        public void Construct(uint splitSectorLevel, 
            uint splitTileLevel, 
            float sectorSize,
            float tileSize, 
            Sector sectorPrefab,
            Tile tilePrefab)
        {
            _sectorSize = sectorSize;

            int lineSize = (int)Math.Pow(2, splitSectorLevel);
            uint tileCount = (uint) Math.Pow(2, splitTileLevel); 

            _sectors = new Sector[lineSize, lineSize];

            float offset = -lineSize* _sectorSize / 2;
            Vector2 mapOffset = new Vector2(offset, offset);

            for (int i = 0; i < _sectors.GetLength(0); i++)
            {
                for (int j = 0; j < _sectors.GetLength(1); j++)
                {
                    _sectors[i, j] = InstantiateSector(new Coords(i, j), mapOffset, sectorPrefab);
                    _sectors[i, j].ConstructTile(tileCount, tilePrefab, tileSize);
                }
            }
        }

        public void ConstructSectorPart(uint splitSectorLevel,
            uint lineSize,
            float sectorSize,
            Vector2 mapOffset,
            Sector sectorPrefab)
        {
            _splitLevel = splitSectorLevel;
            _sectorSize = sectorSize;

            _sectors = new Sector[lineSize, lineSize];

            for (int i = 0; i < _sectors.GetLength(0); i++)
            {
                for (int j = 0; j < _sectors.GetLength(1); j++)
                {
                    _sectors[i, j] = InstantiateSector(new Coords(i, j), mapOffset, sectorPrefab);
                }
            }
        }

        public void ConstructTilePart(uint tileCount, Tile tilePrefab, float tileSize)
        {
            for (int i = 0; i < _sectors.GetLength(0); i++)
            {
                for (int j = 0; j < _sectors.GetLength(1); j++)
                {
                    _sectors[i, j].ConstructTile(tileCount, tilePrefab, tileSize);
                }
            }
        }

        private Sector InstantiateSector(Coords coords, Vector2 mapOffset, Sector sectorPrefab)
        {
            Sector sector = Instantiate<Sector>(sectorPrefab);
            sector.transform.parent = gameObject.transform;

            Rect sectorBounds = ComputeSectorBounds(coords, mapOffset);

            sector.Construct(sectorBounds, coords);

            return sector;
        }

        private Rect ComputeSectorBounds(Coords coords, Vector2 mapOffset)
        {
            Rect sectorBounds = new Rect();

            sectorBounds.position = new Vector2(coords.abs * _sectorSize, 
                                                coords.ord * _sectorSize);

            sectorBounds.position += mapOffset;

            sectorBounds.size = new Vector2(_sectorSize, _sectorSize);

            return sectorBounds;
        }

        public bool IsInitialized()
        {
            return _isInitialized;
        }

        public Sector Get(int x, int y)
        {
            return _sectors[x, y];
        }
    }
}
