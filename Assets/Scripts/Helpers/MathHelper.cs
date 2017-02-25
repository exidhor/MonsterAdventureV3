using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public static class MathHelper
    {
        public static Vector2 GetKinematicMovement_MinCheck(Vector2 direction, float speed, float min)
        {
            if (direction.sqrMagnitude > min)
            {
                direction.Normalize();
                direction *= speed;
            }
            else
            {
                direction = Vector2.zero;
            }

            return direction;
        }

        // source : http://stackoverflow.com/questions/4780119/2d-euclidean-vector-rotations
        public static Vector2 RotateVector(Vector2 vector2, float angleInRadian)
        {
            float cos = Mathf.Cos(angleInRadian);
            float sin = Mathf.Sin(angleInRadian);

            Vector2 result = new Vector2();

            result.x = vector2.x*cos - vector2.y*sin;
            result.y = vector2.x*sin + vector2.y*cos;

            return result;
        }

        public static Vector2 GetDirectionFromAngle(float angleInRadian)
        {
            return new Vector2(Mathf.Sin(angleInRadian),
                Mathf.Cos(angleInRadian));
        }
    }
}
