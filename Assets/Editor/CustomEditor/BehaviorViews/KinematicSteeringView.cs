using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public abstract class KinematicSteeringView
    {
        protected FloatField _pendingMaxSpeed;

        private bool _showContent = true;

        //private KinematicSteering _steering;

        protected KinematicSteeringView()
        {
            _pendingMaxSpeed = new FloatField(0);
        }

        public void Initialize(KinematicSteering steering)
        {
            RevertFrom(steering);
        }

        public void DisplayOnGUI(AIComponent AIComponent)
        {
            _showContent = EditorGUILayout.Foldout(_showContent, GetTitle());

            EditorGUI.indentLevel++;
            if (_showContent)
            {
                DisplayContent();

                if (GUILayout.Button("Apply"))
                {
                    ApplyOn(AIComponent);
                }

                if (GUILayout.Button("Revert"))
                {
                    RevertFrom(AIComponent.GetSteering());
                }
            }

            EditorGUI.indentLevel--;
        }

        public virtual void Actualize(KinematicSteering steering)
        {
            _pendingMaxSpeed.SetValue(steering.GetMaxSpeed());
        }

        protected virtual void DisplayContent()
        {
            _pendingMaxSpeed.DisplayOnGUI("Max Speed");
        }

        protected abstract string GetTitle();

        //protected abstract KinematicSteering GetSteering();

        public abstract EBehavior GetBehavior();

        protected abstract void ApplyOn(AIComponent AIComponent);

        protected virtual void RevertFrom(KinematicSteering steering)
        {
            _pendingMaxSpeed.SetValue(steering.GetMaxSpeed());
        }
    }
}
