using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace MonsterAdventure.Editor
{
    [CustomEditor(typeof(InGameIdComponent))]
    public class InGameIdComponentEditor : UnityEditor.Editor
    {
        private InGameIdComponent _id;

        void OnEnable()
        {
            _id = (InGameIdComponent) serializedObject.targetObject;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.SelectableLabel("mask : " + _id.Id.GetMask());

            DrawBit("Alive", "Dead", InGameId.ALIVE);
            DrawBit("Mineral", "Organic", InGameId.MINERAL);

            // ...
        }

        private bool DrawBit(string name, string contrary, InGameIdValue value)
        {
            bool result = EditorGUILayout.Toggle(GetFullName(name, contrary), _id.Id.Contains(value));

            _id.Id.Set(value, result);

            return result;
        }

        private string GetFullName(string value, string contrary)
        {
            return value + " (!= " + contrary + " )";
        }
    }
}
