using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class Lake : MonoBehaviour
    {
        private List<Sector> _sectors;

        private void Awake()
        {
            _sectors = new List<Sector>();
        }

        public void AddSector(Sector sector)
        {
            _sectors.Add(sector);            
        }

        public void Absorb(Lake lake)
        {
            for (int i = 0; i < lake._sectors.Count; i++)
            {
                _sectors.Add(lake._sectors[i]);
            }

            lake._sectors.Clear();
        }

        public List<Sector> GetSectors()
        {
            return _sectors;
        }
    }
}
