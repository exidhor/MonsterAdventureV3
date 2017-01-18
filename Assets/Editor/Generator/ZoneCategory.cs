using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class ZoneCategory : Category
    {
        private SectorCategory _sectorCategory;
        private ZoneManager _zoneManager;

        private bool _drawZone;

        private int _zoneIndex;

        private int _zoneCount;

        public ZoneCategory(EditorWindow editorWindow, SectorCategory sectorCategory, bool startingHidden = false)
            : base(editorWindow, "Zones", startingHidden)
        {
            _sectorCategory = sectorCategory;
        }

        protected override void DrawContent()
        {
            _drawZone = EditorGUILayout.Toggle("Draw Zone", _drawZone);

            if (_drawZone)
            {
                _zoneIndex = EditorGUILayout.IntSlider(_zoneIndex, 0, _zoneCount - 1);
                EditorGUILayout.SelectableLabel("Zone : " + _zoneManager.GetZones()[_zoneIndex].ToString());
            }
        }

        protected override bool TryToInit()
        {
            _zoneManager = GameObject.FindGameObjectWithTag("ZoneManager").GetComponent<ZoneManager>();

            if (!_zoneManager.isInitialized)
            {
                return false;
            }

            _zoneCount = _zoneManager.GetZones().Count;

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
            if (_drawZone)
            {  
                DrawZone();
            }
        }

        private void DrawZone()
        {
            List<Sector> sectorInCurrentZone = _zoneManager.GetZones()[_zoneIndex].GetSectors();

            Color zoneColor = Color.gray;

            for (int i = 0; i < sectorInCurrentZone.Count; i++)
            {
                GizmosHelper.DrawFillRect(sectorInCurrentZone[i].GetBounds(), zoneColor);
            }
        }
    }
}