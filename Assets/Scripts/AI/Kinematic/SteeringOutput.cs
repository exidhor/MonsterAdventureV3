﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;

namespace MonsterAdventure.AI
{
    [Serializable]
    public class SteeringOutput
    {
        public Vector2 Linear;
        public bool IsKinematic;

        public SteeringOutput()
        {
            Linear = Vector2.zero;
            IsKinematic = true;
        }

        public SteeringOutput(Vector2 linear, bool isKinematic)
        {
            Linear = linear;
            IsKinematic = isKinematic;
        }
    }
}
