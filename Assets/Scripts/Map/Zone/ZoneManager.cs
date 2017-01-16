using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class ZoneManager : MonoBehaviour
    {
        public Zone zonePrefab;

        private List<Zone> _zones;

        public bool isInitialized = false;

        public void Construct()
        {
            _zones = new List<Zone>();
        }

        public void Generate(FakeDoubleEntryList<Sector> sectors)
        {
            _zones.Clear();

            for (int y = 0; y < sectors.lineSize; y++)
            {
                for (int x = 0; x < sectors.lineSize; x++)
                {
                    FindZone(x, y, sectors);
                }
            }

            isInitialized = true;
        }

        private Zone InstantiateZone(Biome biome)
        {
            Zone zone = Instantiate<Zone>(zonePrefab);
            zone.transform.parent = gameObject.transform;
            zone.name = biome.ToString();

            return zone;
        }

        private void FindZone(int x, int y, FakeDoubleEntryList<Sector> sectors)
        {
            Sector currentSector = sectors.GetElement(x, y);
            BiomeList biomeList = currentSector.GetBiomeList();

            if (biomeList.Count != 1)
                return;

            Biome currentBiome = biomeList.GetFirst();

            bool zoneSet = false;

            // check for left
            if (x > 0 && currentBiome == sectors.GetElement(x - 1, y).GetBiomeList().GetFirst())
            {
                Zone zone = sectors.GetElement(x - 1, y).GetZone();
                zone.Add(currentSector);

                zoneSet = true;
            }

            // check for bot
            if (y > 0 && currentBiome == sectors.GetElement(x, y - 1).GetBiomeList().GetFirst())
            {
                Zone zone = sectors.GetElement(x, y - 1).GetZone();

                if (zoneSet)
                {
                    if (zone != currentSector.GetZone())
                    {
                        currentSector.GetZone().Absorb(zone);
                        _zones.Remove(zone);
                        Destroy(zone.gameObject);
                    }
                }
                else
                {
                    zone.Add(currentSector);
                }

                zoneSet = true;
            }

            if (!zoneSet)
            {
                // create a new Zone
                Zone newZone = InstantiateZone(currentBiome);
                newZone.Construct(currentBiome);
                _zones.Add(newZone);

                // add the tile to the zone
                newZone.Add(currentSector);
            }
        }

        public List<Zone> GetZones()
        {
            return _zones;
        }
    }
}
