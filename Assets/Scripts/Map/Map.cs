using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterAdventure
{
    public class Map : MonoBehaviour
    {
        public int tileSize;

        public RandomGenerator randomGenerator;
        public SectorManager sectorManager;
        public BiomeManager biomeManager;
        public LakeManager lakeManager;
        public ZoneManager zoneManager;

        public TileManager tileManager;
        public MovableGrid movableGrid;

        private Rect _bounds;

        private void Awake()
        {
             // nothing 
        }

        private void ConstructBounds()
        {
            float size = (float)Math.Pow(2, sectorManager.resolution) * tileSize;

            Vector2 mapPosition = new Vector2(-size / 2, -size / 2);

            _bounds = new Rect(mapPosition, new Vector2(size, size));
        }

        public void Construct()
        {
            ConstructBounds();

            randomGenerator.Construct();
            sectorManager.Construct(_bounds);
            biomeManager.Construct();
            zoneManager.Construct();
            lakeManager.Construct(sectorManager);
            tileManager.Construct(sectorManager.GetLastSectors(), tileSize, randomGenerator, sectorManager); 
        }

        public void Generate()
        {
            biomeManager.Generate(sectorManager.GetAllSectors(), randomGenerator);
            zoneManager.Generate(sectorManager.GetLastSectors());
            lakeManager.Generate(randomGenerator);
            tileManager.Generate(sectorManager.GetLastSectors());

            movableGrid.Construct(sectorManager);

            int startCoords = (int)sectorManager.GetSectors(movableGrid.targetLevel).lineSize;

            startCoords /= 2;

            movableGrid.SetPosition(startCoords, startCoords);
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
