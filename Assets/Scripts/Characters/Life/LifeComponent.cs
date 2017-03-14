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

        public float Resistance;
        [SerializeField]
        private float LifePerBubble;
        public int MaxLife;
        public float Life;
        public float RegenerationPerSecond;

        public bool IsAlive
        {
            get { return Life > 0; }
        }

        public float ExtractSoulBubble()
        {
            float lifeExtracted = LifePerBubble;

            Life -= lifeExtracted;

            if (!IsAlive)
            {
                lifeExtracted = -Life;
                Life = 0;
            }

            return lifeExtracted;
        }
    }
}
