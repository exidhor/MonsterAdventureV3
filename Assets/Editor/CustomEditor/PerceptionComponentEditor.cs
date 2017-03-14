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

        [DrawGizmo(GizmoType.NonSelected)]
        static void DrawGizmo(PerceptionComponent perception, GizmoType type)
        {
            DrawCircle(perception);
            DrawArc(perception);
        }

        void OnSceneGUI()
        {
            // Draw();
        }

        private static void DrawArc(PerceptionComponent comp)
        {
            //PerceptionComponent comp = (PerceptionComponent)target;

            Arc arc = comp.VisionDetection;

            Handles.color = Color.blue;

            Vector2 arcCenter = arc.Center;

            //Vector2 startArc = GizmosHelper.ConvertToGUICoordinate(arc.Start);
            Vector2 startArc = arc.Start;
            //Vector2 endArc = GizmosHelper.ConvertToGUICoordinate(arc.End);
            Vector2 endArc = arc.End;
            //Vector2 arcCenter = GizmosHelper.ConvertToGUICoordinate(arc.Center);

            Handles.DrawWireArc(arcCenter,
                Vector3.forward,
                startArc - arcCenter,
                arc.MarginAngle.Offset*2,
                arc.Radius);

            Handles.DrawLine(arcCenter, startArc);
            Handles.DrawLine(arcCenter, endArc);
        }

        private static void DrawCircle(PerceptionComponent comp)
        {
            Circle circle = comp.AutoDetection;

            Handles.color = Color.red;

            Handles.DrawWireDisc(circle.Center, Vector3.forward, circle.Radius);
        }
    }
}