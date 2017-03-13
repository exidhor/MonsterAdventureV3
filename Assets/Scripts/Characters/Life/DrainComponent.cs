using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class DrainComponent : MonoBehaviour
    {
        private CircleCollider2D _range;

        void Awake()
        {
            _range = GetComponent<CircleCollider2D>();
        }


    }
}
