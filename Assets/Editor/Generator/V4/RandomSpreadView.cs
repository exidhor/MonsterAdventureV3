using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.Generation;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class RandomSpreadView : GenerationMethodView
    {
        private bool _drawRandomColors;

        public RandomSpreadView(EditorWindow editorWindow, RandomSpread randomSpread, SectorView sectorView, bool startingHidden = false)
            : base(editorWindow, randomSpread, sectorView, startingHidden)
        {
            // nothing
        }

        protected override void DrawContent()
        {
            base.DrawContent();

            _drawRandomColors = EditorGUILayout.Toggle("Draw Random Colors", _drawRandomColors);
        }

        protected override void DrawGizmosContent()
        {
            if (_drawRandomColors)
            {
                _sectorView.DrawOnSectors(_generationMethod.GetLevel(), DrawRandomColors);
            }

            base.DrawGizmosContent();
        }

        private void DrawRandomColors(Rect sectorRect, int x, int y)
        {
            float sample = (float)_generationMethod.GetToken(x, y).GetFloatValue();

            Color color = ((RandomSpread)_generationMethod).GetDebugGradient().Evaluate(sample);

            GizmosHelper.DrawFillRect(sectorRect, color);
        }
    }
}
