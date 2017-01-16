using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class SectorCategory : Category
    {
        private SectorManager _sectorManager;
        private BiomeManager _biomeManager;
        private LakeManager _lakeManager;
        private MovableGrid _movableGrid;

        private bool _drawGrid;
        private string[] _resolutionOptions = null;
        private int _resolutionIndex = 0;

        private bool _drawCoords;

        public SectorCategory(EditorWindow window, bool startingHidden = false) 
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

            _biomeManager = GameObject.FindGameObjectWithTag("BiomeManager").GetComponent<BiomeManager>();

            _lakeManager = GameObject.FindGameObjectWithTag("LakeManager").GetComponent<LakeManager>();

            _movableGrid = GameObject.FindGameObjectWithTag("Map").GetComponent<MovableGrid>();

            if (!_sectorManager.IsInitialized())
            {
                return false;
            }

            List<FakeDoubleEntryList<Sector>> sectors = _sectorManager.GetAllSectors();
            _resolutionOptions = new string[sectors.Count];

            for (int i = 0; i < sectors.Count; i++)
            {
                _resolutionOptions[i] = GetResolutionName(i, sectors[i].singleEntryList.Count);

                if (i == _biomeManager.GetTargetLevel())
                {
                    _resolutionOptions[i] += " (biomes)";
                }

                if (i == _lakeManager.targetLevel)
                {
                    _resolutionOptions[i] += " (lakes)";
                }

                if (i == _movableGrid.targetLevel)
                {
                    _resolutionOptions[i] += " (movableGrid)";
                }
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

        public override void DrawGizmosContent()
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
            int name = (int)Math.Sqrt(count);

            return "[" + index + "] " + name + "x" + name;
        }

        private void DrawGrid()
        {
            Color gridColor = Color.gray;

            List<Sector> currentSectors = _sectorManager.GetAllSectors()[_resolutionIndex].singleEntryList;

            //Rect bufferGUIRect;

            foreach (Sector sector in currentSectors)
            {
                //bufferGUIRect = GizmosHelper.ConvertToGUICoordinate(sector.GetBounds());

                //GizmosHelper.DrawRect(bufferGUIRect, gridColor, 1);
                GizmosHelper.DrawRect(sector.GetBounds(), gridColor, 1);
            }
        }

        /*private void DrawCoords()
        {
            FakeDoubleEntryList<Sector> list = _sectorManager.GetSectors(_resolutionIndex);

            for (int i = 0; i < list.lineSize; i++)
            {
                for (int j = 0; j < list.lineSize; j++)
                {
                    String coordStrings = "(" + j + ", " + i + ")";

                    Handles.Label(list.GetElement(i, j).GetPosition(), coordStrings);
                }
            }
        }*/

        private void DrawCoords()
        {
            FakeDoubleEntryList<Sector> list = _sectorManager.GetSectors(_resolutionIndex);

            //Vector2 GUIPosition;

            for (int i = 0; i < list.lineSize; i++)
            {
                for (int j = 0; j < list.lineSize; j++)
                {
                    Coords coords = list.GetElement(i, j).GetCoords();

                    String coordStrings = "(" + coords.abs + ", " + coords.ord + ")";

                    //GUIPosition = GizmosHelper.ConvertToGUICoordinate(list.GetElement(i, j).GetPosition());

                    //Handles.Label(GUIPosition, coordStrings);
                    GizmosHelper.DrawLabel(list.GetElement(i, j).GetPosition(), coordStrings);
                }
            }
        }

        public int GetResolutionIndex()
        {
            return _resolutionIndex;
        }
    }
}
