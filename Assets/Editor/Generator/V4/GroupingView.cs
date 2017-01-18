using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.Generation;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class GroupingView : GenerationMethodView
    {
        private bool _drawValueColors;

        public GroupingView(EditorWindow editorWindow, Grouping grouping, SectorView sectorView, bool startingHidden = false)
            : base(editorWindow, grouping, sectorView, startingHidden)
        {
            // nothing
        }

        protected override void DrawContent()
        {
            base.DrawContent();

            _drawValueColors = EditorGUILayout.Toggle("Draw Value Colors", _drawValueColors);
        }

        protected override void DrawGizmosContent()
        {
            if (_drawValueColors)
            {
                _sectorView.DrawOnSectors(_generationMethod.GetLevel(), DrawValueColors);
            }

            base.DrawGizmosContent();
        }

        private void DrawValueColors(Rect sectorRect, int x, int y)
        {
            int indexNoiseValue = _generationMethod.GetToken(x, y).GetIntValue();

            Color color = ((Grouping) _generationMethod).GetColor(indexNoiseValue);

            GizmosHelper.DrawFillRect(sectorRect, color);
        }
    }
}