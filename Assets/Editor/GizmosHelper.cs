using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace MonsterAdventure.Editor
{
    public static class GizmosHelper
    {
        public static void DrawRect(Rect rect, Color color, float z = 0)
        {
            rect = ConvertToGUICoordinate(rect);

            Gizmos.color = color;

            Vector2[] verts = GetVerticesFromRect(rect);

            for (int i = 0; i < verts.Length - 1; i++)
            {
                Gizmos.DrawLine(verts[i], verts[i+1]);
            }

            Gizmos.DrawLine(verts.Last(), verts[0]);
        }

        public static void DrawFillRect(Rect rect, Color color)
        {
            rect = ConvertToGUICoordinate(rect);
            Handles.DrawSolidRectangleWithOutline(rect, color, color);
        }

        public static void DrawLabel(Vector3 position, string text)
        {
            position = ConvertToGUICoordinate(position);

            Handles.Label(position, text);
        }

        public static void DrawLabel(Rect rect, string text)
        {
            DrawLabel(rect.center, text);
        }

        private static Vector2[] GetVerticesFromRect(Rect rect)
        {
            Vector2[] verts = new Vector2[4];

            Vector2 topLeftCorner = new Vector2(rect.xMin, rect.yMin);
            Vector2 topRightCorner = new Vector2(rect.xMax, rect.yMin);
            Vector2 botRightCorner = new Vector2(rect.xMax, rect.yMax);
            Vector2 botLeftCorner = new Vector2(rect.xMin, rect.yMax);

            verts[0] = topLeftCorner;
            verts[1] = topRightCorner;
            verts[2] = botRightCorner;
            verts[3] = botLeftCorner;

            return verts;
        }

        public static Rect ConvertToGUICoordinate(Rect rect)
        {
            Rect GUIRect = new Rect(rect);
            GUIRect.y = -GUIRect.y;

            GUIRect.y -= GUIRect.height;

            return GUIRect;
        }

        public static Vector2 ConvertToGUICoordinate(Vector2 vector2)
        {
            Vector2 GUIVector2 = new Vector2(vector2.x, vector2.y);
            GUIVector2.y = -GUIVector2.y;

            return GUIVector2;
        }
    }
}