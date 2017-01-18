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
        private bool _drawValues;

        protected GenerationMethod _generationMethod;
        protected SectorView _sectorView;

        public GenerationMethodView(EditorWindow editorWindow, GenerationMethod generationMethod, SectorView sectorView,
            bool startingHidden = false)
            : base(editorWindow, generationMethod.GetName(), startingHidden)
        {
            _generationMethod = generationMethod;
            _sectorView = sectorView;
        }

        protected override void DrawContent()
        {
            _drawValues = EditorGUILayout.Toggle("Draw values", _drawValues);
        }

        protected override bool TryToInit()
        {
            if (_sectorView.IsInitialized())
            {
                _sectorView.AddName(_generationMethod.GetName(), _generationMethod.GetLevel());

                return true;
            }

            return false;
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
                _sectorView.DrawOnSectors(_generationMethod.GetLevel(), DrawValue);
            }
        }

        private void DrawValue(Rect sectorRect, int x, int y)
        {
            GizmosHelper.DrawLabel(sectorRect, _generationMethod.GetToken(x, y).GetDebugLabel(typeof(float)));
        }
    }
}