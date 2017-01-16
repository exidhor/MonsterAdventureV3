using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class Zone : MonoBehaviour
    {
        private Biome _biome;
        private List<Sector> _sectors;


        private void Awake()
        {
            // nothing
        }

        public void Construct(Biome biome)
        {
            _sectors = new List<Sector>();
            _biome = biome;
        }

        public void Add(Sector sector)
        {
            // add the tile to the zone
            _sectors.Add(sector);

            // and set the zone to the tile
            sector.SetZone(this);
        }

        public void Absorb(Zone zone)
        {
            _sectors.AddRange(zone._sectors);

            for (int i = 0; i < zone._sectors.Count; i++)
            {
                zone._sectors[i].SetZone(this);
            }
        }

        public override string ToString()
        {
            return gameObject.name;
        }

        public List<Sector> GetSectors()
        {
            return _sectors;
        }
    }
}
