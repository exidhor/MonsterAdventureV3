using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;

namespace MonsterAdventure.Editor
{
    public class MovableGridCategory : Category
    {
        private bool _drawActualSectors;

        private MovableGrid _movableGrid;

        public MovableGridCategory(EditorWindow editorWindow, bool startingHidden = false)
            : base(editorWindow, "Movable Grid", startingHidden)
        {
            // nothing ?
        }

        protected override void DrawContent()
        {
            _drawActualSectors = EditorGUILayout.Toggle("Draw Actual Sectors", _drawActualSectors);
        }

        protected override bool TryToInit()
        {
            _movableGrid = GameObject.FindGameObjectWithTag("Map").GetComponent<MovableGrid>();

            if (!_movableGrid.isInitialized)
            {
                return false;
            }

            return true;
        }

        protected override void UpdateContent()
        {
           // nothing
        }

        protected override void ResetContent()
        {
            _movableGrid = null;
        }

        protected override void DrawGizmosContent()
        {
            if (_drawActualSectors)
            {
                DrawActualSectors();
            }
        }

        private void DrawActualSectors()
        {
            Sector[,] sectorGrid = _movableGrid.GetSectorGrid();

            Color gridColor = Color.black;

            //Rect bufferGUIRect;

            for (int i = 0; i < sectorGrid.GetLength(0); i++)
            {
                for (int j = 0; j < sectorGrid.GetLength(1); j++)
                {
                    //bufferGUIRect = GizmosHelper.ConvertToGUICoordinate(sectorGrid[i, j].GetBounds()); 
                    //bufferGUIRect = sectorGrid[i, j].GetBounds(); 

                    //GizmosHelper.DrawRect(bufferGUIRect, gridColor, 1);
                    GizmosHelper.DrawRect(sectorGrid[i, j].GetBounds(), gridColor, 1);
                }
            } 
        }
    }
}
