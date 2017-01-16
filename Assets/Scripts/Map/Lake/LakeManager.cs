using System;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterAdventure
{
    [RequireComponent(typeof(NoiseGenerator))]
    public class LakeManager : MonoBehaviour
    {
        [Range(0f, 1f)]
        public float offset;

        [Range(0f, 1f)]
        public float rate;

        public int targetLevel;

        public Lake lakePrefab;

        public Gradient wetRateGradient;

        private List<Lake> _lakes;

        private bool _isInitiliazed;

        private NoiseGenerator _noiseGenerator;
        private SectorManager _sectorManager;

        private void Awake()
        {
            _noiseGenerator = GetComponent<NoiseGenerator>();

            _isInitiliazed = false;
        }

        public void Construct(SectorManager sectorManager)
        {
            _lakes = new List<Lake>();

            _sectorManager = sectorManager;

            _noiseGenerator.Construct();
        }

        public void Generate(RandomGenerator randomGenerator)
        {
            int mapSize = (int)_sectorManager.GetSectors(targetLevel).lineSize;

            _noiseGenerator.Generate(mapSize, transform, randomGenerator);

            ApplyNoise(_sectorManager.GetSectors(targetLevel));

            CreateLakes();
        }

        private void ApplyNoise(FakeDoubleEntryList<Sector> sectors)
        {
            for (int i = 0; i < sectors.lineSize; i++)
            {
                for (int j = 0; j < sectors.lineSize; j++)
                {
                    float wetRate = _noiseGenerator.Get(i, j);


                    
                    sectors.GetElement(i, j).SetWetRate(wetRate, true, true);
                }
            }
        }

        private void CreateLakes()
        {
            FakeDoubleEntryList<Sector> sectors = _sectorManager.GetSectors(targetLevel);

            _lakes.Clear();

            for (int i = 0; i < sectors.lineSize; i++)
            {
                for (int j = 0; j < sectors.lineSize; j++)
                {
                    FindLake(i, j, sectors);
                }
            }
        }

        private void FindLake(int x, int y, FakeDoubleEntryList<Sector> sectors)
        {
            Sector currentSector = sectors.GetElement(x, y);

            bool isLake = IsLake(currentSector.GetWetRate());

            if (!isLake)
                return;

            // check for left
            if (x > 0 && sectors.GetElement(x-1, y).GetLake())
            {
                Lake lake = sectors.GetElement(x-1, y).GetLake();
                currentSector.SetLake(lake);
                lake.AddSector(currentSector);
            }

            // check for bot
            if (y > 0 && sectors.GetElement(x, y-1).GetLake())
            {
                Lake lake = sectors.GetElement(x, y - 1).GetLake();

                if (currentSector.GetLake())
                {
                    if (lake != currentSector.GetLake())
                    {
                        currentSector.GetLake().Absorb(lake);
                        _lakes.Remove(lake);
                        Destroy(lake.gameObject);
                    }
                }
                else
                {
                    currentSector.SetLake(lake);
                    lake.AddSector(currentSector);
                }
            }

            if (!currentSector.GetLake())
            {
                // create a new Zone
                Lake newLake = InstantiateLake();
                _lakes.Add(newLake);

                // add the tile to the zone
                newLake.AddSector(currentSector);

                // set the zone to the tile
                currentSector.SetLake(newLake);
            }
        }

        private bool IsLake(float wetRate)
        {
            return offset <= wetRate && wetRate <= offset + rate;
        }

        private Lake InstantiateLake()
        {
            Lake lake = Instantiate<Lake>(lakePrefab);
            lake.transform.parent = gameObject.transform;
            lake.name = "Lake";

            return lake;
        }

        public List<Lake> GetLakes()
        {
            return _lakes;
        }
    }
}
