using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class SectorManager : MonoBehaviour
    {
        [Range(0, 12)]
        public int resolution;

        private List<FakeDoubleEntryList<Sector>> _sectorPerLevel;

        private bool _isInitialized = false;

        private void Awake()
        {
            // nothing
        }

        public void Construct(Rect mapBounds)
        {
            // construct the sector list
            _sectorPerLevel = new List<FakeDoubleEntryList<Sector>>();
            
            // init with the first sector (the big parent)
            int currentLevel = 0;

            _sectorPerLevel.Add(new FakeDoubleEntryList<Sector>(0));

            _sectorPerLevel[currentLevel].singleEntryList.Add(new Sector(new Coords(0, 0), 
                                                                         (uint)currentLevel, 
                                                                         mapBounds));  
            
            // Start the division
            _sectorPerLevel[currentLevel].singleEntryList[0].Divide((uint)resolution);
            
            // retrieve then store all the sectors created by the division
            List<Sector[]> childrenStack = new List<Sector[]>();
            List<Sector[]> childrenStack_next = new List<Sector[]>();
             
            childrenStack.Add(_sectorPerLevel[currentLevel].singleEntryList[0].GetChildren());
            currentLevel++;

            while (childrenStack.Count > 0)
            {
                // we create a new list to store the current children
                _sectorPerLevel.Add(new FakeDoubleEntryList<Sector>(0));

                // we add the stack to the sector list
                foreach (Sector[] children in childrenStack)
                {
                    foreach (Sector child in children)
                    {
                        _sectorPerLevel[currentLevel].singleEntryList.Add(child);

                        if (currentLevel < resolution)
                        {
                            childrenStack_next.Add(child.GetChildren());
                        }
                    } 
                }

                // we swap the stacks
                childrenStack = new List<Sector[]>(childrenStack_next);
                childrenStack_next.Clear();

                // we increase the level
                currentLevel++;
            }

            SectorComparer sectorComparer = new SectorComparer();

            // we reorder the sectors to improve speed access
            for (int i_level = 0; i_level < _sectorPerLevel.Count; i_level++)
            {
                _sectorPerLevel[i_level].singleEntryList.Sort(sectorComparer);
            }

            // we set the lineSize into the FakeList
            for (int i_level = 0; i_level < _sectorPerLevel.Count; i_level++)
            {
                _sectorPerLevel[i_level].lineSize = (uint)Math.Sqrt(_sectorPerLevel[i_level].singleEntryList.Count);
            }

            _isInitialized = true;

            Debug.Log("SectorManager constructed");
        }

        public List<FakeDoubleEntryList<Sector>> GetAllSectors()
        {
            return _sectorPerLevel;
        }

        public FakeDoubleEntryList<Sector> GetSectors(int level)
        {
            return _sectorPerLevel[level];
        }

        public FakeDoubleEntryList<Sector> GetLastSectors()
        {
            return _sectorPerLevel.Last();
        }

        public Sector Get(int x, int y, int level)
        {
            return _sectorPerLevel[level].GetElement(x, y);
        }

        public bool IsInitialized()
        {
            return _isInitialized;
        }
    }
}
