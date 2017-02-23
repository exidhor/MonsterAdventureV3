using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class TransformField
    {
        public Transform Value
        {
            get { return _value; }

            set
            {
                if (_value != value)
                {
                    _value = value;
                    _hasChanged = true;
                }
            }
        }

        public bool HasChanged
        {
            get { return _hasChanged; }
        }

        private Transform _value;
        private bool _hasChanged = false;

        public TransformField(Transform value)
        {
            _value = value;
        }

        public void DisplayOnGUI(string label)
        {
            Value = (Transform)EditorGUILayout.ObjectField(label, Value, typeof(Transform), true);
        }

        private string GetCurrentLabel(string label)
        {
            if (HasChanged)
            {
                return label + "*";
            }

            return label;
        }

        public void SetValue(Transform value)
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
