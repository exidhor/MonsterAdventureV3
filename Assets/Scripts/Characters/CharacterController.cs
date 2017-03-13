using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MonsterAdventure.AI;
using UnityEngine;

namespace MonsterAdventure
{
    [RequireComponent(typeof(InGameIdComponent))]
    public class CharacterController : MonoBehaviour
    {
        private InGameIdComponent _inGameIdComponent;
        [SerializeField]
        private Kinematic _kinematic;
        
        void Awake()
        {
            _inGameIdComponent = GetComponent<InGameIdComponent>();
        }

        public InGameId GetInGameId()
        {
            return _inGameIdComponent.Id;
        }

        public Kinematic GetKinematic()
        {
            return _kinematic;
        }
    }
}
