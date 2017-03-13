using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class LifeComponent : MonoBehaviour
    {
        public Transform AbsorptionPoint;

        public int MaxLife;
        public float Life;
        public float RegenerationPerSecond;
    }
}
