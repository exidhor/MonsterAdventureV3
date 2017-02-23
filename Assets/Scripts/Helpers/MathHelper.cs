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
    }
}
