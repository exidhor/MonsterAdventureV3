
/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class GenerationGridView : View
    {
        public delegate void ToDrawOnGrid(Rect boxRect, int x, int y);

        protected GeneratorView GeneratorView
        {
            get { return (GeneratorView)_editorWindow; }
        }

        private bool _drawGrid;
        private string[] _resolutionOptions = null;
        private int[] _resolutionValues;
        private int _resolutionIndex = 0;

        private SectorManager _sectorManager;

        private bool _drawCoords;

        public GenerationGridView(GeneratorView window, bool startingHidden = false)
            : base(window, "GenerationGrid", startingHidden)
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
            Generation.Generator generator = GeneratorView.GetGenerator();
            _sectorManager = GeneratorView.GetSectorManager();

            if (generator == null || !generator.IsInitialized()
                || _sectorManager == null || !_sectorManager.IsInitialized())
            {
                return false;
            }

            uint maxLevel = generator.GetMaxSplitLevel();

            _resolutionOptions = new string[maxLevel];
            _resolutionValues = new int[maxLevel];

            for (int i = 0; i < _resolutionOptions.Length; i++)
            {
                _resolutionValues[i] = (int)Math.Pow(2, i);
                _resolutionOptions[i] = GetResolutionName(i, _resolutionValues[i]);
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
            int name = (int)Math.Sqrt(count);

            return "[" + index + "] " + name + "x" + name;
        }

        public void AddName(string name, uint level)
        {
            _resolutionOptions[level] += " (" + name + ")";
        }

        private void DrawGrid()
        {
            Color gridColor = Color.gray;

            int resolution = _resolutionValues[_resolutionIndex];
            float offset = 0;

            for(int i = 0; i < resolution; i++)
            {
                //GizmosHelper.DrawLine();
            }
        }

        private void DrawCoords()
        {
            /*
            FakeDoubleEntryList<Sector> list = _sectorManager.GetSectors(_resolutionIndex);

            for (int i = 0; i < list.lineSize; i++)
            {
                for (int j = 0; j < list.lineSize; j++)
                {
                    Coords coords = list.GetElement(i, j).GetCoords();

                    String coordStrings = "(" + coords.abs + ", " + coords.ord + ")";

                    GizmosHelper.DrawLabel(list.GetElement(i, j).GetPosition(), coordStrings);
                }
            }*//*
        }

        public void DrawOnGrid(uint level, ToDrawOnGrid toDrawOnGrid)
        {
            /*FakeDoubleEntryList<Sector> sectorsList = _sectorManager.GetSectors(_resolutionIndex);

            uint lineSize = sectorsList.lineSize;

            for (int x = 0; x < lineSize; x++)
            {
                for (int y = 0; y < lineSize; y++)
                {
                    toDrawOnGrid(sectorsList.GetElement(x, y).GetBounds(), x, y);
                }
            }*//*
        }

        public int GetResolutionIndex()
        {
            return _resolutionIndex;
        }
    }
}
*/