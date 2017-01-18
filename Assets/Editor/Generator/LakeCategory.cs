using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class LakeCategory : Category
    {
        private SectorManager _sectorManager;

        private SectorCategory _sectorCategory;
        private LakeManager _lakeManager;

        private bool _drawWetRate;
        private bool _drawColorWetRate;
        private Gradient _gradientWetRate;
        private bool _drawLake;
        private Color _lakeColor = Color.blue;

        public LakeCategory(EditorWindow editorWindow, SectorCategory sectorCategory, bool startingHidden = false)
            : base(editorWindow, "Lakes", startingHidden)
        {
            _sectorCategory = sectorCategory;
        }

        protected override void DrawContent()
        {
            _drawWetRate = EditorGUILayout.Toggle("Draw wet rate", _drawWetRate);
            _drawColorWetRate = EditorGUILayout.Toggle("Draw Color Wet Rate", _drawColorWetRate);

            _drawLake = EditorGUILayout.Toggle("Draw lake", _drawLake);

            if (_drawLake)
            {
                _lakeColor = EditorGUILayout.ColorField("Lake color", _lakeColor);
            }
        }

        protected override bool TryToInit()
        {
            _sectorManager = GameObject.FindGameObjectWithTag("SectorManager").GetComponent<SectorManager>();
            _lakeManager = GameObject.FindGameObjectWithTag("LakeManager").GetComponent<LakeManager>();

            if (!_sectorManager.IsInitialized())
            {
                return false;
            }

            return true;
        }

        protected override void UpdateContent()
        {
            // todo
        }

        protected override void ResetContent()
        {
            // todo
        }

        protected override void DrawGizmosContent()
        {
            if(_drawColorWetRate)
                DrawColorWetRate();

            if (_drawLake)
                DrawLake();

            if (_drawWetRate)
                DrawWetRate();
        }

        private void DrawWetRate()
        {
            FakeDoubleEntryList<Sector> list = _sectorManager.GetSectors(_sectorCategory.GetResolutionIndex());

            //Vector2 GUIPosition;

            for (int i = 0; i < list.lineSize; i++)
            {
                for (int j = 0; j < list.lineSize; j++)
                {
                    float wetRate = list.GetElement(i, j).GetWetRate();

                    wetRate = (float) Math.Round((double) wetRate, 2);

                    //Handles.Label(list.GetElement(i, j).GetPosition(), wetRate.ToString());
                    GizmosHelper.DrawLabel(list.GetElement(i, j).GetPosition(), wetRate.ToString());    
                }
            }
        }

        private void DrawColorWetRate()
        {
            FakeDoubleEntryList<Sector> list = _sectorManager.GetSectors(_sectorCategory.GetResolutionIndex());

            //Rect bufferGUIRect;

            for (int i = 0; i < list.lineSize; i++)
            {
                for (int j = 0; j < list.lineSize; j++)
                {
                    float wetRate = list.GetElement(i, j).GetWetRate();

                    wetRate = (float)Math.Round((double)wetRate, 2);

                    //bufferGUIRect = GizmosHelper.ConvertToGUICoordinate(list.GetElement(i, j).GetBounds());
                    //GizmosHelper.DrawFillRect(bufferGUIRect, _lakeManager.wetRateGradient.Evaluate(wetRate));
                    GizmosHelper.DrawFillRect(list.GetElement(i, j).GetBounds(), 
                        _lakeManager.wetRateGradient.Evaluate(wetRate));
                }
            }
        }

        private void DrawLake()
        {
            List<Lake> lakes = _lakeManager.GetLakes();

            //Rect bufferGUIRect;

            for (int i = 0; i < lakes.Count; i++)
            {
                List<Sector> sectors = lakes[i].GetSectors();

                for (int j = 0; j < sectors.Count; j++)
                {
                    //bufferGUIRect = GizmosHelper.ConvertToGUICoordinate(sectors[j].GetBounds());
                    //GizmosHelper.DrawFillRect(bufferGUIRect, _lakeColor);
                    GizmosHelper.DrawFillRect(sectors[j].GetBounds(), _lakeColor);
                }
            }
        }
    }
}