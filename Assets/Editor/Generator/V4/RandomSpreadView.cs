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

        public RandomSpreadView(GeneratorView generatorWindow, RandomSpread randomSpread, GridDisplay gridDisplay, bool startingHidden = false)
            : base(generatorWindow, randomSpread, gridDisplay, startingHidden)
        {
            // nothing
        }

        protected override void DrawValue(int x, int y, out Color color, out string text)
        {
            float sample = (float)_generationMethod.GetToken(x, y).GetFloatValue();

            color = ((RandomSpread)_generationMethod).GetDebugGradient().Evaluate(sample);

            text = _generationMethod.GetToken(x, y).GetDebugLabel(typeof(float));
        }
    }
}
