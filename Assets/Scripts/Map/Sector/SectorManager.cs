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

        public float MapSize
        {
            get { return _mapSize; }
        }

        public float MapOffset
        {
            get { return _mapOffset; }
        }

        private bool _isInitialized;

        private Sector[,] _sectors;
        private float _sectorSize;
        private uint _splitLevel;
        private int _mapSize;
        private float _mapOffset;

        public void Construct(uint splitSectorLevel,  
            int mapSize,
            Sector sectorPrefab)
        {
            _splitLevel = splitSectorLevel;
            _mapSize = mapSize;

            int lineSize = (int)Math.Pow(2, _splitLevel);

            _sectorSize = mapSize / (float)lineSize;

            _sectors = new Sector[lineSize, lineSize];

            _mapOffset = -lineSize * _sectorSize / 2;
            Vector2 mapOffset = new Vector2(_mapOffset, _mapOffset);

            for (int i = 0; i < _sectors.GetLength(0); i++)
            {
                for (int j = 0; j < _sectors.GetLength(1); j++)
                {
                    _sectors[i, j] = InstantiateSector(new Coords(i, j), mapOffset, sectorPrefab);
                }
            }
        }



        /*
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
}*/

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

        public Sector Get(Coords coords)
        {
            return _sectors[coords.abs, coords.ord];
        }

        public void AddTracedObjectToSector(TracedObject tracedObject, Vector2 objectSize = new Vector2())
        {
            GetSectorFromPosition(tracedObject.Position, objectSize).AddTracedObject(tracedObject);
        }

        public Coords GetCoordsFromPosition(Vector2 position, Vector2 objectSize = new Vector2())
        {
            // find the good sector
            Coords coords = new Coords();

            coords.abs = (int)((position.x - _mapOffset - objectSize.x) / _sectorSize);
            coords.ord = (int)((position.y - _mapOffset - objectSize.y) / _sectorSize);

            return coords;
        }

        public Sector GetSectorFromPosition(Vector2 position, Vector2 objectSize)
        {
            return Get(GetCoordsFromPosition(position, objectSize));
        }

        public void DisableEverySectors()
        {
            for (int i = 0; i < _sectors.GetLength(0); i++)
            {
                for (int j = 0; j < _sectors.GetLength(1); j++)
                {
                    _sectors[i, j].SetIsVisible(false);
                }
            }
        }
    }
}
