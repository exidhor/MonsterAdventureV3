using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;
using UnityEditor;

namespace MonsterAdventure.Editor
{
    [CustomEditor(typeof(KinematicSeek))]
    public class KinematicSeekEditor : UnityEditor.Editor
    {
        private KinematicSeekView _view;

        void OnEnable()
        {
            KinematicSeek _seek = (KinematicSeek)serializedObject.targetObject;

            _view = new KinematicSeekView();
        }

        // when Unity displays the editor
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            //_view.DisplayOnGUI();
        }
    }
}
