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

        public NoiseSpreadView(GeneratorView generatorWindow, NoiseSpread noiseSpread, GridDisplay gridDisplay, bool startingHidden = false)
            : base(generatorWindow, noiseSpread, gridDisplay, startingHidden)
        {
            // nothing
        }

        protected override void DrawValue(int x, int y, out Color color, out string text)
        {
            float sample = (float)_generationMethod.GetToken(x, y).GetFloatValue();

            color = ((NoiseSpread) _generationMethod).GetDebugGradient().Evaluate(sample);

            text = _generationMethod.GetToken(x, y).GetDebugLabel(typeof(float));
        }
    }
}
