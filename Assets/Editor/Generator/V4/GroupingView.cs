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

        public GroupingView(GeneratorView generatorWindow, Grouping grouping, GridDisplay gridDisplay, bool startingHidden = false)
            : base(generatorWindow, grouping, gridDisplay, startingHidden)
        {
            // nothing
        }

        protected override void DrawValue(int x, int y, out Color color, out string text)
        {
            int indexNoiseValue = _generationMethod.GetToken(x, y).GetIntValue();

            color = ((Grouping)_generationMethod).GetColor(indexNoiseValue);

            text = _generationMethod.GetToken(x, y).GetDebugLabel(typeof(int));
        }
    }
}