using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.AI
{
    [Serializable]
    public struct BehaviorAndWeight
    {
        public KinematicSteering Steering;
        public float Weight;

        public BehaviorAndWeight(KinematicSteering steering, float weight)
        {
            Steering = steering;
            Weight = weight;
        }
    }
}
