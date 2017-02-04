using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace MonsterAdventure
{
    public enum Direction
    {
        NEGATIVE = -1,
        NONE = 0,
        POSITIVE = 1
    }

    public class MovableGrid : MonoBehaviour
    {
        public bool isInitialized = false;

        private SectorManager _sectorManager;
        private Sector[,] _sectors;
        private Player _player;

        // the bounds which delimit when the movable grid move to follow the player
        private Rect _changementBox;

        // the bounds where delimit when the movable grid teleport to follow the player 
        private Rect _limitsBox;

        private Sector CurrentSector
        {
            get { return _sectors[1, 1]; }
        }

        private void Awake()
        {
            _sectors = new Sector[3, 3];

            _changementBox = new Rect();
            _limitsBox = new Rect();
        }

        private void Start()
        {
        }

        public void Construct(SectorManager sectorManager)
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            _sectorManager = sectorManager;
            
            Coords coords = _sectorManager.GetCoordsFromPosition(_player.transform.position);

            SetPosition(coords);

            isInitialized = true;
        }


        private void ComputeBox()
        {
            _changementBox.size = CurrentSector.Bounds.size*1.5f;
            _changementBox.center = CurrentSector.Bounds.center;

            _limitsBox.size = CurrentSector.Bounds.size*3f;
            _limitsBox.center = CurrentSector.Bounds.center;
        }

        private void Update()
        {
            Vector2 playerPosition = _player.transform.position;

            Coords newCoords = _sectorManager.GetCoordsFromPosition(playerPosition);

            ActualizeGrid(newCoords, playerPosition); 
        }

        void OnDrawGizmosSelected()
        {
            if (CurrentSector != null && isInitialized) // means that the object is constructed
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(CurrentSector.Bounds.center, CurrentSector.Bounds.size);
                Gizmos.DrawWireCube(_changementBox.center, _changementBox.size);
                Gizmos.DrawWireCube(_limitsBox.center, _limitsBox.size);
            }
        }

        private void ActualizeGrid(Coords newCoords, Vector2 playerPosition)
        {
            if (IsPossiblePosition(newCoords))
            {
                if (!_limitsBox.Contains(playerPosition))
                {
                    SetPosition(newCoords);

                    // actualize box 
                    ComputeBox();
                }

                else if (!_changementBox.Contains(playerPosition))
                {
                    Direction abscissDirection;
                    Direction ordinateDirection;

                    GetMoveDirection(newCoords, out abscissDirection, out ordinateDirection);

                    Move(abscissDirection, ordinateDirection);

                    // actualize box 
                    ComputeBox();
                }
            }
        }

        private bool IsPossiblePosition(Coords newCoords)
        {
            return newCoords.abs > 0 && newCoords.ord > 0
                   && newCoords.abs < _sectorManager.LineSize - 1 && newCoords.ord < _sectorManager.LineSize - 1;
        }

        private void GetMoveDirection(Coords newCoords, out Direction abscissDirection, out Direction ordinateDirection)
        {
            Coords currentCoords = GetCenterCoords();

            RelativeCoords relativeCoords = newCoords.GetRelativeCoords(currentCoords);

            int offsetAbs = relativeCoords.VectorToOrigin.abs;
            int offsetOrd = relativeCoords.VectorToOrigin.ord;

            abscissDirection = Direction.NONE;
            ordinateDirection = Direction.NONE;

            if (offsetAbs == 1)
            {
                abscissDirection = Direction.POSITIVE;
            }
            else if (offsetAbs == -1)
            {
                abscissDirection = Direction.NEGATIVE;
            }

            if (offsetOrd == 1)
            {
                ordinateDirection = Direction.POSITIVE;
            }
            else if (offsetOrd == -1)
            {
                ordinateDirection = Direction.NEGATIVE;
            }
        }

        private void SetPosition(Coords coords)
        {
            SetPosition(coords.abs, coords.ord);
        }

        private void SetPosition(int centerCoords_x, int centerCoords_y)
        {
            // Disable current sectors
            for (int i = 0; i < _sectors.GetLength(0); i++)
            {
                for (int j = 0; j < _sectors.GetLength(1); j++)
                {
                    if (_sectors[i, j] != null)
                    {
                        _sectors[i, j].SetIsVisible(false);
                    }
                }
            }
            
            // put the coords to the bot left (make it easier to manipulate)
            centerCoords_x--;
            centerCoords_y--;

            Vector2 currentCoords = new Vector2();

            for (Int32 i = 0; i < _sectors.GetLength(0); i++)
            {
                for (Int32 j = 0; j < _sectors.GetLength(1); j++)
                {
                    currentCoords.x = centerCoords_x + i;
                    currentCoords.y = centerCoords_y + j;

                    _sectors[i, j] = GetNextSector((int) currentCoords.x, (int) currentCoords.y);
                    _sectors[i, j].SetIsVisible(true);
                }
            }
        }

        private void Move(Direction abs, Direction ord)
        {
            // everything is hard coded to improve the perfomance

            if (abs == Direction.POSITIVE)
            {
                // the coords of the top-left corner of the grid
                Int32 newLastCoord_x = _sectors[2, 0].GetCoords().abs + 1;
                Int32 newLastCoord_y = _sectors[2, 0].GetCoords().ord;

                // We rotate the grid
                _sectors[0, 0].SetIsVisible(false);
                _sectors[0, 0] = _sectors[1, 0];
                _sectors[1, 0] = _sectors[2, 0];

                // Set the coords for the new Coords (Only the last line or column)
                _sectors[2, 0] = GetNextSector(newLastCoord_x, newLastCoord_y);

                // repeat this algo for each line or culumn (depending if we check for the abs or the ord)
                // ...

                _sectors[0, 1].SetIsVisible(false);
                _sectors[0, 1] = _sectors[1, 1];
                _sectors[1, 1] = _sectors[2, 1];
                _sectors[2, 1] = GetNextSector(newLastCoord_x, newLastCoord_y + 1);

                _sectors[0, 2].SetIsVisible(false);
                _sectors[0, 2] = _sectors[1, 2];
                _sectors[1, 2] = _sectors[2, 2];
                _sectors[2, 2] = GetNextSector(newLastCoord_x, newLastCoord_y + 2);
            }

            else if (abs == Direction.NEGATIVE)
            {
                Int32 newFirstCoord_x = _sectors[0, 0].GetCoords().abs - 1;
                Int32 newFirstCoord_y = _sectors[0, 0].GetCoords().ord;

                _sectors[2, 0].SetIsVisible(false);
                _sectors[2, 0] = _sectors[1, 0];
                _sectors[1, 0] = _sectors[0, 0];
                _sectors[0, 0] = GetNextSector(newFirstCoord_x, newFirstCoord_y);

                _sectors[2, 1].SetIsVisible(false);
                _sectors[2, 1] = _sectors[1, 1];
                _sectors[1, 1] = _sectors[0, 1];
                _sectors[0, 1] = GetNextSector(newFirstCoord_x, newFirstCoord_y + 1);

                _sectors[2, 2].SetIsVisible(false);
                _sectors[2, 2] = _sectors[1, 2];
                _sectors[1, 2] = _sectors[0, 2];
                _sectors[0, 2] = GetNextSector(newFirstCoord_x, newFirstCoord_y + 2);
            }

            if (ord == Direction.NEGATIVE)
            {
                Int32 newLastCoord_x = _sectors[0, 0].GetCoords().abs;
                Int32 newLastCoord_y = _sectors[0, 0].GetCoords().ord - 1;

                _sectors[0, 2].SetIsVisible(false);
                _sectors[0, 2] = _sectors[0, 1];
                _sectors[0, 1] = _sectors[0, 0];
                _sectors[0, 0] = GetNextSector(newLastCoord_x, newLastCoord_y);

                _sectors[1, 2].SetIsVisible(false);
                _sectors[1, 2] = _sectors[1, 1];
                _sectors[1, 1] = _sectors[1, 0];
                _sectors[1, 0] = GetNextSector(newLastCoord_x + 1, newLastCoord_y);

                _sectors[2, 2].SetIsVisible(false);
                _sectors[2, 2] = _sectors[2, 1];
                _sectors[2, 1] = _sectors[2, 0];
                _sectors[2, 0] = GetNextSector(newLastCoord_x + 2, newLastCoord_y);
            }

            else if (ord == Direction.POSITIVE)
            {
                Int32 newFirstCoord_x = _sectors[0, 2].GetCoords().abs;
                Int32 newFirstCoord_y = _sectors[0, 2].GetCoords().ord + 1;

                _sectors[0, 0].SetIsVisible(false);
                _sectors[0, 0] = _sectors[0, 1];
                _sectors[0, 1] = _sectors[0, 2];
                _sectors[0, 2] = GetNextSector(newFirstCoord_x, newFirstCoord_y);

                _sectors[1, 0].SetIsVisible(false);
                _sectors[1, 0] = _sectors[1, 1];
                _sectors[1, 1] = _sectors[1, 2];
                _sectors[1, 2] = GetNextSector(newFirstCoord_x + 1, newFirstCoord_y);

                _sectors[2, 0].SetIsVisible(false);
                _sectors[2, 0] = _sectors[2, 1];
                _sectors[2, 1] = _sectors[2, 2];
                _sectors[2, 2] = GetNextSector(newFirstCoord_x + 2, newFirstCoord_y);
            }
        }

        public List<Sector> GetNeighbours(UInt32 x, UInt32 y)
        {
            Int32 abs = (Int32) x;
            Int32 ord = (Int32) y;

            List<Sector> neighbours = new List<Sector>();

            for (Int32 i = abs - 1; i <= abs + 1; i++)
            {
                // we skip if the case is outside the grid
                if (i < 0 || i > 2)
                {
                    continue;
                }

                for (Int32 j = ord - 1; j <= ord + 1; j++)
                {
                    // the same : we skip if the case is outside the grid
                    if (j < 0 || j > 2)
                    {
                        continue;
                    }

                    // if we reach this instruction, that mean that the chunk position is possible
                    // according to the bounds of the grid. Then, we can add it
                    neighbours.Add(_sectors[i, j]);
                }
            }

            return neighbours;
        }

        public Sector Get(UInt32 abs, UInt32 ord)
        {
            return _sectors[abs, ord];
        }

        public void Set(UInt32 abs, UInt32 ord, Sector sector)
        {
            _sectors[abs, ord] = sector;
        }

        public Int32 GetLength(Int32 index)
        {
            return _sectors.GetLength(index);
        }

        private Sector GetNextSector(int x, int y)
        {
            if (x < 0 || y < 0)
                return null;

            Sector newSector = _sectorManager.Get(x, y);

            newSector.SetIsVisible(true);

            return newSector;
        }

        public Sector[,] GetSectorGrid()
        {
            return _sectors;
        }

        public Coords GetCenterCoords()
        {
            return _sectors[1, 1].GetCoords();
        }
    }
}