using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;
using UnityEngine;

namespace MonsterAdventure
{
    [RequireComponent(typeof(Collider2D))]
    public class Presence : MonoBehaviour
    {
        private InGameIdComponent _inGameIdComponent;

        public GameObject Parent
        {
            get { return transform.parent.gameObject; }
        }

        public Vector2 Position
        {
            get { return Parent.transform.position; }
        }

        public Kinematic Kinematic
        {
            get { return Parent.GetComponent<Kinematic>(); }
        }

        public InGameId InGameId
        {
            get { return _inGameIdComponent.Id; }
        }

        void Awake()
        {
            _inGameIdComponent = GetComponentInParent<InGameIdComponent>();
        }
    }
}
