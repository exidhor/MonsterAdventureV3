using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;
using UnityEditor;

namespace MonsterAdventure.Editor
{
    [CustomEditor(typeof(AIComponent))]
    public class AIComponentEditor : UnityEditor.Editor
    {
        private AIComponent _AIComponent;

        private bool _isInit = false;

        void OnEnable()
        {
            //_AIComponent = (AIComponent)serializedObject.targetObject;

        }

        // when Unity displays the editor
        public override void OnInspectorGUI()
        {
            if(!_isInit)
            {
                Init();   
            }

            if (_isInit)
            {
                base.OnInspectorGUI();

                DisplayKinematicSteering(_AIComponent.Behavior);
            }
        }

        private void DisplayKinematicSteering(EBehavior behavior)
        {
            switch (behavior)
            {                
                case EBehavior.Seek:
                    DisplaySeek((KinematicSeek) _AIComponent.GetSteering());
                    break;

                default:
                    // nothing
                    break;
            }
        }

        private void Init()
        {
            int instanceIdInScene = ((AIComponent)serializedObject.targetObject).GetInstanceIdInScene();

            _AIComponent = (AIComponent)EditorUtility.InstanceIDToObject(instanceIdInScene);

            if (_AIComponent != null)
            {
                _isInit = true;
            }
        }

        private void DisplaySeek(KinematicSeek seek)
        {
            if (seek != null)
            {
                DisplayMaxSpeed(seek);
                DisplayTarget(seek);
            }
        }

        private void DisplayMaxSpeed(KinematicSteering steering)
        {
            steering.SetMaxSpeed(EditorGUILayout.FloatField("Max Speed", steering.GetMaxSpeed())); 
        }

        private void DisplayTarget(TargetedKinematicSteering steering)
        {
            SerializableLocation.DisplayOnGUI("Target", steering.Target);
        }
    }
}
