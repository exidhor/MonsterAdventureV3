using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    [Serializable]
    public class LifeTrace
    {
        public LifeData LifeData = null;

        /// <summary>
        /// Actualize datas with the time ellapsed during 
        /// the sleep mode
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Activate(float deltaTime, LifeComponent lifeComponent)
        {
            if (LifeData != null)
            {
                LifeData.Regenerate(deltaTime);
                lifeComponent.LifeData.Copy(LifeData);
            }
        }

        public void Disable(LifeComponent lifeComponent)
        {
            if (LifeData != null)
            {
                LifeData.Copy(lifeComponent.LifeData);
            }
        }
    }
}