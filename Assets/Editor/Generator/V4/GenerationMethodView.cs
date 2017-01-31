using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.Generation;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class GenerationMethodView : View
    {
        protected GeneratorView GeneratorView
        {
            get { return (GeneratorView) _editorWindow; }
        }

        private bool _drawValues;
        private bool _lastState;
        private bool _hasChanged;

        protected GenerationMethod _generationMethod;
        protected GridDisplay _gridView;

        public GenerationMethodView(GeneratorView generatorWindow, 
            GenerationMethod generationMethod, 
            GridDisplay gridView,
            bool startingHidden = false)

            : base(generatorWindow, generationMethod.GetName(), startingHidden)
        {
            _generationMethod = generationMethod;
            _gridView = gridView;

            _lastState = _drawValues;
        }

        protected override void DrawContent()
        {
            _drawValues = EditorGUILayout.Toggle("Draw values", _drawValues);
        }

        protected override bool TryToInit()
        {
            return true;
        }

        protected override void UpdateContent()
        {
            if (_lastState != _drawValues)
            {
                _hasChanged = true;
            }
            else
            {
                _hasChanged = false;
            }

            _lastState = _drawValues;
        }

        protected override void ResetContent()
        {
            // todo
        }

        protected override void DrawGizmosContent()
        {
            if (_drawValues)
            {
                uint size = (uint) Math.Pow(2, _generationMethod.GetLevel());
                _gridView.SetDatas(size, DrawValue);
            }
        }

        protected virtual void DrawValue(int x, int y, out Color color, out string text)
        {
            text = _generationMethod.GetToken(x, y).GetDebugLabel(typeof(float));
            color = Color.white;
        }

        public void DisableDraw()
        {
            _drawValues = false;
            _lastState = false;
        }

        public bool HasChanged()
        {
            return _hasChanged;
        }
    }
}