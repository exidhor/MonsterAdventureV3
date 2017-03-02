using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{

    [CustomEditor(typeof(PerceptionComponent))]
    public class PerceptionComponentEditor : UnityEditor.Editor
    {
        private Arc _arc;

        void OnSceneGUI()
        {
            PerceptionComponent comp = (PerceptionComponent) target;

            Handles.color = Color.blue;

            _arc = comp.VisionDetection;

            //Vector2 startArc = GizmosHelper.ConvertToGUICoordinate(arc.Start);
            Vector2 startArc = _arc.Start;
            //Vector2 endArc = GizmosHelper.ConvertToGUICoordinate(arc.End);
            Vector2 endArc = _arc.End;
            //Vector2 arcCenter = GizmosHelper.ConvertToGUICoordinate(arc.Center);
            Vector2 arcCenter = _arc.Center;

            Handles.DrawWireArc(arcCenter, 
                Vector3.forward, 
                startArc - arcCenter,
                _arc.MarginAngle.Offset * 2,
                _arc.Radius);

            Handles.DrawLine(arcCenter, startArc);
            Handles.DrawLine(arcCenter, endArc);
        }
    }
}
