using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class SectorView : View
    {
        public delegate void DrawOnSector(Rect sectorRect, int x, int y);

        private SectorManager _sectorManager;

        private bool _drawGrid;
        private string[] _resolutionOptions = null;
        private int _resolutionIndex = 0;

        private bool _drawCoords;

        public SectorView(EditorWindow window, bool startingHidden = false)
            : base(window, "Sector", startingHidden)
        {
            // nothing ? 
        }

        protected override void DrawContent()
        {
            if (_resolutionOptions != null)
            {
                _resolutionIndex = EditorGUILayout.Popup(_resolutionIndex, _resolutionOptions);
            }

            _drawGrid = EditorGUILayout.Toggle("Draw grid", _drawGrid);

            _drawCoords = EditorGUILayout.Toggle("Draw Coords", _drawCoords);
        }

        protected override bool TryToInit()
        {
            _sectorManager = GameObject.FindGameObjectWithTag("SectorManager").GetComponent<SectorManager>();

            if (!_sectorManager.IsInitialized())
            {
                return false;
            }

            List<FakeDoubleEntryList<Sector>> sectors = _sectorManager.GetAllSectors();
            _resolutionOptions = new string[sectors.Count];

            for (int i = 0; i < sectors.Count; i++)
            {
                _resolutionOptions[i] = GetResolutionName(i, sectors[i].singleEntryList.Count);
            }

            return true;
        }

        protected override void UpdateContent()
        {
            // todo
        }

        protected override void ResetContent()
        {
            _resolutionOptions = null;
        }

        protected override void DrawGizmosContent()
        {
            if (_drawGrid)
            {
                DrawGrid();
            }

            if (_drawCoords)
            {
                DrawCoords();
            }
        }

        private string GetResolutionName(int index, int count)
        {
            int name = (int) Math.Sqrt(count);

            return "[" + index + "] " + name + "x" + name;
        }

        public void AddName(string name, uint level)
        {
            _resolutionOptions[level] += " (" + name + ")";
        }

        private void DrawGrid()
        {
            Color gridColor = Color.gray;

            List<Sector> currentSectors = _sectorManager.GetAllSectors()[_resolutionIndex].singleEntryList;

            foreach (Sector sector in currentSectors)
            {
                GizmosHelper.DrawRect(sector.GetBounds(), gridColor, 1);
            }
        }

        private void DrawCoords()
        {
            FakeDoubleEntryList<Sector> list = _sectorManager.GetSectors(_resolutionIndex);

            for (int i = 0; i < list.lineSize; i++)
            {
                for (int j = 0; j < list.lineSize; j++)
                {
                    Coords coords = list.GetElement(i, j).GetCoords();

                    String coordStrings = "(" + coords.abs + ", " + coords.ord + ")";

                    GizmosHelper.DrawLabel(list.GetElement(i, j).GetPosition(), coordStrings);
                }
            }
        }

        public void DrawOnSectors(uint level, DrawOnSector drawOnSector)
        {
            FakeDoubleEntryList<Sector> sectorsList = _sectorManager.GetSectors(_resolutionIndex);

            uint lineSize = sectorsList.lineSize;

            for (int x = 0; x < lineSize; x++)
            {
                for (int y = 0; y < lineSize; y++)
                {
                    drawOnSector(sectorsList.GetElement(x, y).GetBounds(), x, y);
                }
            }
        }

        public int GetResolutionIndex()
        {
            return _resolutionIndex;
        }
    }
}