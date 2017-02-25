using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public class KinematicField
    {
        public Kinematic Value
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

        private Kinematic _value;
        private bool _hasChanged = false;

        public KinematicField(Kinematic value)
        {
            _value = value;
        }

        public void DisplayOnGUI(string label)
        {
            Value = (Kinematic)EditorGUILayout.ObjectField(label, Value, typeof(Kinematic), true);
        }

        private string GetCurrentLabel(string label)
        {
            if (HasChanged)
            {
                return label + "*";
            }

            return label;
        }

        public void SetValue(Kinematic value)
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
