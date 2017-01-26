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
        }

        protected override void DrawContent()
        {
            _drawValues = EditorGUILayout.Toggle("Draw values", _drawValues);
        }

        protected override bool TryToInit()
        {
            /*if (_gridView.IsInitialized())
            {
                _gridView.AddName(_generationMethod.GetName(), _generationMethod.GetLevel());

                return true;
            }

            return false;*/

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
    }
}