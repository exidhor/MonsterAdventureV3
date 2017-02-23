using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class Vector2Field
    {
        public Vector2 Value
        {
            get { return _value; }

            set
            {
                _value = value;
                _hasChanged = true;
            }
        }

        public bool HasChanged
        {
            get { return _hasChanged; }
        }

        private Vector2 _value;
        private bool _hasChanged = false;

        public Vector2Field(Vector2 value)
        {
            _value = value;
        }

        public void DisplayOnGUI(string label)
        {
            Value = EditorGUILayout.Vector2Field(GetCurrentLabel(label), Value);
        }

        private string GetCurrentLabel(string label)
        {
            if (HasChanged)
            {
                return label + "*";
            }

            return label;
        }

        public void SetValue(Vector2 value)
        {
            _value = value;
            _hasChanged = false;
        }

        public void Reset()
        {
            _hasChanged = false;
        }
    }
}
