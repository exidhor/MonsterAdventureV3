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

        private KinematicSteeringView _steeringView;

        private EBehavior _oldBehavior;

        private EBehavior _pendingBehavior;
        private bool _autoRevert;

        void OnEnable()
        {
            _AIComponent = (AIComponent)serializedObject.targetObject;

            Revert();
        }

        // when Unity displays the editor
        public override void OnInspectorGUI()
        {
            HandleAutoRevert();

            if(ViewNeedConstruction())
            {
                ConstructSteeringView();
            }

            _autoRevert = EditorGUILayout.Toggle("Auto revert on change", _autoRevert);

            _pendingBehavior = (EBehavior) EditorGUILayout.EnumPopup("Behavior", _pendingBehavior);

            if (_pendingBehavior != EBehavior.None)
            {
                _AIComponent.OutputBuffer.Linear = EditorGUILayout.Vector2Field("Output buffer", _AIComponent.OutputBuffer.Linear);

                if (_steeringView != null)
                {
                    _steeringView.DisplayOnGUI(_AIComponent);
                }
            }
        }

        private void HandleAutoRevert()
        {
            if (!_autoRevert)
                return;

            if (_oldBehavior != _AIComponent.Behavior)
            {
                Revert();
            }
            else
            {
                ActualizeViewSteering();   
            }
        }

        private void ActualizeViewSteering()
        {
            KinematicSteering steering = _AIComponent.GetSteering();

            if (_steeringView != null && steering != null)
            {
                _steeringView.Actualize(steering);
            }
        }

        private bool ViewNeedConstruction()
        {
            return (_steeringView == null && _pendingBehavior != EBehavior.None)
                   || (_steeringView != null && _pendingBehavior != _steeringView.GetBehavior());
        }

        private void Revert()
        {
            _oldBehavior = _AIComponent.Behavior;
            _pendingBehavior = _AIComponent.Behavior;

            ConstructSteeringView();
        }

        //private void ChangeSteeringInAIComponent()
        //{
        //    switch (_AIComponent.Behavior)
        //    {
        //        case EBehavior.None:
        //            _AIComponent.RemoveSteering();
        //            break;

        //        case EBehavior.Seek:
        //            _AIComponent.AddSeekSteering();
        //    }
        //}

        private void ConstructSteeringView()
        {
            //KinematicSteering steering = _AIComponent.GetSteering();
            //EBehavior behavior = _AIComponent.Behavior;

            switch (_pendingBehavior)
            {
                case EBehavior.None:
                    _steeringView = null;
                    break;

                case EBehavior.Seek:
                    _steeringView = new KinematicSeekView();
                    break;

                case EBehavior.Flee:
                    _steeringView = new KinematicFleeView();
                    break;

                case EBehavior.Arrive:
                    _steeringView = new KinematicArriveView();
                    break;

                case EBehavior.Wander:
                    _steeringView = new KinematicWanderView();
                    break;

                case EBehavior.Face:
                    _steeringView = new KinematicFaceView();
                    break;

                case EBehavior.Pursue:
                    _steeringView = new KinematicPursueView();
                    break;

                case EBehavior.Evade:
                    _steeringView = new KinematicEvadeView();
                    break;

                case EBehavior.DelegateWander:
                    _steeringView = new KinematicDelegateWanderView();
                    break;
            }

            KinematicSteering currentSteering = _AIComponent.GetSteering();

            if (currentSteering != null)
            {
                //_steeringView.Initialize(currentSteering);
            }
        }

        //private void DisplayKinematicSteering(EBehavior behavior)
        //{
        //    switch (behavior)
        //    {                
        //        case EBehavior.Seek:
        //            DisplaySeek((KinematicSeek) _AIComponent.GetSteering());
        //            break;

        //        default:
        //            // nothing
        //            break;
        //    }
        //}

        //private void DisplaySeek(KinematicSeek seek)
        //{
        //    if (seek != null)
        //    {
        //        DisplayMaxSpeed(seek);
        //        DisplayTarget();
        //    }
        //}

        //private void DisplayMaxSpeed(KinematicSteering steering)
        //{
        //    steering.SetMaxSpeed(EditorGUILayout.FloatField("Max Speed", steering.GetMaxSpeed())); 
        //}

        //private void DisplayTarget()
        //{
        //    _targetLocation.DisplayOnGUI("Target");
        //}
    }
}
