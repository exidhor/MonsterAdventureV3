using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    public class SteeringPool<T> : MonoBehaviour
    {
        private List<T> _pool;

        private void Start()
        {
            _pool = new List<T>();
        }

        private void GetFreeElement()
        {
            
        }
    }
}
