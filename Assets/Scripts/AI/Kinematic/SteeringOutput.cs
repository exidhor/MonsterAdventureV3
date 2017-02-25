using System;
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
        public bool IsKinematic;
        public bool IsOriented;

        public Vector2 Linear;

        public float AngularInDegree;

        public float AngularInRadian
        {
            get { return AngularInDegree * Mathf.Deg2Rad;}
            set { AngularInDegree = value * Mathf.Rad2Deg; }
        }

        public SteeringOutput()
        {
            Reset();
        }

        public SteeringOutput(Vector2 linear, float angularInDegree, bool isKinematic)
        {
            Linear = linear;
            AngularInDegree = angularInDegree;
            IsKinematic = isKinematic;
        }

        public void Reset()
        {
            Linear = Vector2.zero;
            AngularInDegree = 0;
            IsKinematic = true;
        }
    }
}
