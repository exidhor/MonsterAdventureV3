using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace MonsterAdventure.Editor
{
    public class EnumField
    {
        public Enum Value
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

        private Enum _value;
        private bool _hasChanged = false;

        public EnumField(Enum value)
        {
            _value = value;
        }

        public void DisplayOnGUI(string label)
        {
            Value = EditorGUILayout.EnumPopup(GetCurrentLabel(label), Value);
        }

        private string GetCurrentLabel(string label)
        {
            if (HasChanged)
            {
                return label + "*";
            }

            return label;
        }

        public void SetValue(Enum value)
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
