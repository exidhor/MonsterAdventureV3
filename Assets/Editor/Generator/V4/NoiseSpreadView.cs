using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.Generation;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class NoiseSpreadView : GenerationMethodView
    {
        private bool _drawNoiseColors;

        public NoiseSpreadView(EditorWindow editorWindow, NoiseSpread noiseSpread, SectorView sectorView, bool startingHidden = false)
            : base(editorWindow, noiseSpread, sectorView, startingHidden)
        {
            // nothing
        }

        protected override void DrawContent()
        {
            base.DrawContent();

            _drawNoiseColors = EditorGUILayout.Toggle("Draw Noise Colors", _drawNoiseColors);
        }

        protected override void DrawGizmosContent()
        {
            if (_drawNoiseColors)
            {
                _sectorView.DrawOnSectors(_generationMethod.GetLevel(), DrawNoiseColors);
            }

            base.DrawGizmosContent();
        }

        private void DrawNoiseColors(Rect sectorRect, int x, int y)
        {
            float sample = (float)_generationMethod.GetToken(x, y).GetFloatValue();

            Color color = ((NoiseSpread) _generationMethod).GetDebugGradient().Evaluate(sample);

            GizmosHelper.DrawFillRect(sectorRect, color);
        }
    }
}
