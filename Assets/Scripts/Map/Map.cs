using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class Map : MonoBehaviour
    {
        public Generation.Generator generator;

        public Tile tilePrefab;
        public Sprite baseTileSprite;
        public Sector sectorPrefab;

        [Range(0, 10)] public uint splitSectorLevel;
        [Range(0, 10)] public uint splitTileLevel;


        public SectorManager SectorManager
        {
            get { return _sectorManager; }
        }

        public TileManager TileManager
        {
            get { return _tileManager; }
        }

        public RandomGenerator RandomGenerator
        {
            get { return _randomGenerator;}
        }

        public Rect Bounds
        {
            get { return _bounds; }
        }


        private Rect _bounds;

        private TileManager _tileManager;
        private SectorManager _sectorManager;
        private RandomGenerator _randomGenerator;

        private Vector2 _mapOffset;

        public void Construct(RandomGenerator randomGenerator)
        {
            _randomGenerator = randomGenerator;

            CreateManagers();

            //float tileSize =  baseTileSprite.rect.width;
            float tileSize =  1f;

            float sectorSize = (float) Math.Pow(2, splitTileLevel)*tileSize;

            uint lineSize = (uint)Math.Pow(2, splitSectorLevel);
            uint tileCount = (uint)Math.Pow(2, splitTileLevel);

            float offset = -lineSize * sectorSize / 2;
            Vector2 mapOffset = new Vector2(offset, offset);

            _sectorManager.ConstructSectorPart(splitSectorLevel,
                lineSize,
                sectorSize,
                mapOffset,
                sectorPrefab);

            _sectorManager.ConstructTilePart(tileCount, tilePrefab, tileSize);

            generator.Construct();
        }

        private void CreateManagers()
        {
            GameObject goSectorManger = new GameObject("SectorManager", typeof(SectorManager));
            goSectorManger.transform.parent = transform;
            _sectorManager = goSectorManger.GetComponent<SectorManager>();

            GameObject goTileManager = new GameObject("TileManager", typeof(TileManager));
            goTileManager.transform.parent = transform;
            _tileManager = goTileManager.GetComponent<TileManager>();
        }
    }
}