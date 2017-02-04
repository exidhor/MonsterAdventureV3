using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterAdventure
{
    public class Map : MonoBehaviour
    {
        public Text CoordText;

        public Generation.Generator generator;

        public Sector sectorPrefab;

        [Range(0, 10)] public uint splitSectorLevel;
        public uint size;
        [Range(0, 10)] public uint splitTileLevel;


        public SectorManager SectorManager
        {
            get { return _sectorManager; }
        }

        public RandomGenerator RandomGenerator
        {
            get { return _randomGenerator; }
        }

        public Rect Bounds
        {
            get { return _bounds; }
        }


        private Rect _bounds;

        private SectorManager _sectorManager;
        private RandomGenerator _randomGenerator;
        private MovableGrid _movableGrid;

        private Player _player;

        private Vector2 _mapOffset;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        public void Construct(RandomGenerator randomGenerator)
        {
            _randomGenerator = randomGenerator;

            CreateManagers();

            //float tileSize =  baseTileSprite.rect.width;
            float tileSize = 1f;

            float sectorSize = (float) Math.Pow(2, splitTileLevel)*tileSize;

            uint lineSize = (uint) Math.Pow(2, splitSectorLevel);
            uint tileCount = (uint) Math.Pow(2, splitTileLevel);

            float offset = -lineSize*sectorSize/2;
            Vector2 mapOffset = new Vector2(offset, offset);

            _sectorManager.Construct(splitSectorLevel, (int) size, sectorPrefab);

            /*
            _sectorManager.ConstructSectorPart(splitSectorLevel,
                lineSize,
                sectorSize,
                mapOffset,
                sectorPrefab);

            */

            //_sectorManager.ConstructTilePart(tileCount, tilePrefab, tileSize);

            generator.Construct();

            _movableGrid = GameObject.FindGameObjectWithTag("MovableGrid").GetComponent<MovableGrid>();
            _movableGrid.Construct(_sectorManager);
        }

        private void Update()
        {
            Vector2 playerPosition = _player.transform.position;

            Coords newCoords = _sectorManager.GetCoordsFromPosition(playerPosition);

            CoordText.text = "Coords (" + newCoords.abs + ", " + newCoords.ord + ")";
        }

        private void CreateManagers()
        {
            GameObject goSectorManger = new GameObject("SectorManager", typeof(SectorManager));
            goSectorManger.transform.parent = transform;
            _sectorManager = goSectorManger.GetComponent<SectorManager>();
        }
    }
}