using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class Sector
    {
        private static uint ChildrenNumber = 4;

        private Tile _tile;

        private Coords _coords;
        private Vector2 _centeredPosition;
        private Rect _bounds;
        private BiomeList _biomeList;
        private int _distanceToLimit;

        private uint _level;
        private Sector _parent;
        private Sector[] _children;

        private Lake _lake;
        private float _wetRate;

        private Zone _zone;

        private bool _active;

        public Sector(Coords coords, uint level, Rect bounds)
            : this(coords, level, bounds, null)
        {
            // nothing
        }

        private Sector(Coords coords, uint level, Rect bounds, Sector parent)
        {
            _coords = coords;

            _bounds = bounds;
            _biomeList = new BiomeList();

            _distanceToLimit = -1;

            _level = level;
            _parent = parent;
            _children = null;

            _lake = null;
            _wetRate = 0;

            ComputeCenteredPosition();

            _active = false;
        }

        public void Divide(uint maxLevel)
        {
            if (_level < maxLevel)
            {
                _children = new Sector[ChildrenNumber];

                Rect[] childBounds = ComputeChildBounds();
                Coords[] childCoords = ComputeChildCoords();

                for (int i = 0; i < _children.Length; i++)
                {
                    _children[i] = new Sector(childCoords[i], _level + 1, childBounds[i], this);
                    _children[i].Divide(maxLevel);
                }
            }
        }

        public void SetDistanceToLimit(int distanceToLimit)
        {
            _distanceToLimit = distanceToLimit;
        }

        public int GetDistanceToLimit()
        {
            return _distanceToLimit;
        }

        public Coords GetCoords()
        {
            return _coords;
            ;
        }

        public BiomeList GetBiomeList()
        {
            return _biomeList;
        }

        private float ApplyModifierOnWetRate(float wetRate)
        {
            if (_biomeList.GetBiomeContainers().Count == 1)
            {
                wetRate += _biomeList.GetBiomeContainers()[0].biome.data.wetModifier;

                if (wetRate < 0)
                    wetRate = 0;
                else if (wetRate > 1)
                    wetRate = 1;
            }

            return wetRate;
        }

        public void SetWetRate(float wetRate, bool spreadToDescendants, bool spreadToAncestors)
        {
            wetRate = ApplyModifierOnWetRate(wetRate);

            _wetRate = wetRate;

            // Transmit the wet rate to the descendants
            if (spreadToDescendants)
            {
                SpreadWetRateToDescendants(wetRate);
            }

            // Transmit the wet rate to the ancestors
            if (spreadToAncestors)
            {
                SpreadWetRateToAncestors(wetRate);
            }
        }

        private void SpreadWetRateToDescendants(float wetRate)
        {
            // Transmit the biome to the descendants
            if (_children != null)
            {
                foreach (Sector child in _children)
                {
                    child._wetRate = wetRate;

                    // we transmit only to the descendants because we dont want infinite calls
                    child.SpreadWetRateToDescendants(wetRate);
                }
            }
        }

        private void SpreadWetRateToAncestors(float wetRate)
        {
            // Transmit the biome to the ancestors
            if (_parent != null)
            {
                _parent._wetRate += wetRate/ChildrenNumber;

                // we transmit only to the descendants because we dont want infinite calls
                _parent.SpreadWetRateToAncestors(wetRate/ChildrenNumber);
            }
        }

        public void AddBiome(Biome biome, bool spreadToDescendants, bool spreadToAncestors)
        {
            _biomeList.AddBiome(biome, 1);

            // Transmit the biome to the descendants
            if (spreadToDescendants)
            {
                SpreadBiomeToDescendants(biome);
            }

            // Transmit the biome to the ancestors
            if (spreadToAncestors)
            {
                SpreadBiomeToAncestors(biome);
            }
        }

        private void SpreadBiomeToDescendants(Biome biome, float startingRatio = 1)
        {
            // Transmit the biome to the descendants
            if (_children != null)
            {
                foreach (Sector child in _children)
                {
                    child._biomeList.AddBiome(biome, startingRatio);

                    // we transmit only to the descendants because we dont want infinite calls
                    child.SpreadBiomeToDescendants(biome, startingRatio);
                }
            }
        }

        private void SpreadBiomeToAncestors(Biome biome, float startingRatio = 1)
        {
            // Transmit the biome to the ancestors
            if (_parent != null)
            {
                float newRatio = startingRatio/ChildrenNumber;

                _parent._biomeList.AddBiome(biome, newRatio);

                // we transmit only to the descendants because we dont want infinite calls
                _parent.SpreadBiomeToAncestors(biome, newRatio);
            }
        }

        private void ComputeCenteredPosition()
        {
            _centeredPosition.x = _bounds.x + _bounds.width/2;
            _centeredPosition.y = _bounds.y + _bounds.height/2;
        }

        private Rect[] ComputeChildBounds()
        {
            Rect[] childBounds = new Rect[ChildrenNumber];

            Vector2 dividedSize = _bounds.size/2;

            for (int i = 0; i < childBounds.Length; i++)
            {
                childBounds[i] = new Rect(_centeredPosition, dividedSize);
            }

            // left - bot
            childBounds[0].x -= dividedSize.x;

            // right - bot
            // nothing to change

            // left - top
            childBounds[2].x -= dividedSize.x;
            childBounds[2].y -= dividedSize.y;

            // right - top
            childBounds[3].y -= dividedSize.y;

            return childBounds;
        }

        private Coords[] ComputeChildCoords()
        {
            Coords[] childCoords = new Coords[ChildrenNumber];

            // left bot
            childCoords[0] = new Coords(_coords.abs*(int) ChildrenNumber/2 + 0,
                _coords.ord*(int) ChildrenNumber/2 + 0);

            // right bot
            childCoords[1] = new Coords(_coords.abs*(int) ChildrenNumber/2 + 1,
                _coords.ord*(int) ChildrenNumber/2 + 0);

            // left top
            childCoords[2] = new Coords(_coords.abs*(int) ChildrenNumber/2 + 0,
                _coords.ord*(int) ChildrenNumber/2 + 1);

            // right top
            childCoords[3] = new Coords(_coords.abs*(int) ChildrenNumber/2 + 1,
                _coords.ord*(int) ChildrenNumber/2 + 1);

            return childCoords;
        }

        public static bool BelongToBiomeLimit(Sector sector, FakeDoubleEntryList<Sector> sectors)
        {
            if (sectors.singleEntryList.Count == 0)
                return false;

            if (sector._biomeList.Count != 1)
                return true;

            Biome currentBiome = sector._biomeList.GetFirst();
             
            int current_x = sector._coords.abs;
            int current_y = sector._coords.ord;

            // check at the left
            if (current_x > 0
                && sectors.GetElement(current_x - 1, current_y)._biomeList.GetFirst() != currentBiome)
            {
                return true;
            }

            // check top
            if (current_y < sectors.lineSize - 1
                && sectors.GetElement(current_x, current_y + 1)._biomeList.GetFirst() != currentBiome)
            {
                return true;
            }

            // check right
            if (current_x < sectors.lineSize - 1
                && sectors.GetElement(current_x + 1, current_y)._biomeList.GetFirst() != currentBiome)
            {
                return true;
            }

            // check bot
            if (current_y > 0
                && sectors.GetElement(current_x, current_y - 1)._biomeList.GetFirst() != currentBiome)
            {
                return true;
            }

            return false;
        }

        public Sector[] GetChildren()
        {
            return _children;
        }

        public Sector GetParent()
        {
            return _parent;
        }

        public Rect GetBounds()
        {
            return _bounds;
        }

        public Vector2 GetPosition()
        {
            return _centeredPosition;
        }

        public float GetWetRate()
        {
            return _wetRate;
        }

        public Lake GetLake()
        {
            return _lake;
        }

        public void SetLake(Lake lake)
        {
            _lake = lake;
        }

        public void SetActive(bool state)
        {
            if (_children != null)
            {
                for (int i = 0; i < _children.Length; i++)
                {
                    _children[i].SetActive(state);
                }
            }

            else if (_tile != null)
            {
                _tile.SetIsVisible(state);
            }
        }

        public List<Tile> GetAllTiles()
        {
            if (_tile != null)
            {
                List<Tile> tileList = new List<Tile>();
                tileList.Add(_tile);
                return tileList;
            }

            if (_children == null)
            {
                return new List<Tile>();
            }

            List<Tile> tiles = new List<Tile>();

            foreach (Sector child in _children)
            {
                tiles.AddRange(child.GetAllTiles());
            }

            return tiles;
        }

        public Tile GetTile()
        {
            return _tile;
        }

        public void SetTile(Tile tile)
        {
            _tile = tile;
        }

        public override string ToString()
        {
            return _coords.ToString();
        }

        public void SetZone(Zone zone)
        {
            _zone = zone;
        }

        public Zone GetZone()
        {
            return _zone;
        }
    }
}