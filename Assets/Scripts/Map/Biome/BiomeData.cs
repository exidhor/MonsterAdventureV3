using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    public class BiomeData : MonoBehaviour
    {
        public Color color;
        public float ratio;
        public float wetModifier;
        public Sprite[] sprites;

        public void NormalizeRatio(float ratioSum)
        {
            ratio /= ratioSum;
        } 
    }
}