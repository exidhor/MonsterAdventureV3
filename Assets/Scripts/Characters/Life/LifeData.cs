using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    [Serializable]
    public class LifeData
    {
        public float Resistance;
        public float LifePerBubble;
        public int MaxLife;
        public float Regeneration;

        public float Life
        {
            get { return _life; }
            set { SetLife(value); }
        }

        public bool IsAlive
        {
            get { return Life > float.Epsilon; }
        }

        [SerializeField] private float _life;

        public void Regenerate(float time)
        {
            if (IsAlive)
            {
                Life += Regeneration*time;
            }
        }

        private void SetLife(float newLife)
        {
            if (newLife < 0)
            {
                _life = 0;
            }
            else if (newLife > MaxLife)
            {
                _life = MaxLife;
            }
            else
            {
                _life = newLife;
            }
        }

        public void Copy(LifeData lifeData)
        {
            Resistance = lifeData.Resistance;
            LifePerBubble = lifeData.LifePerBubble;
            MaxLife = lifeData.MaxLife;
            Regeneration = lifeData.Regeneration;
        }

        public void WriteOn(LifeData lifeData)
        {
            lifeData.Copy(this);
        }
    }
}