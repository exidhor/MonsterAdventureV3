using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    [RequireComponent(typeof(PerceptionComponent), typeof(Targeter))]
    public class IntelligenceComponent : MonoBehaviour
    {
        private PerceptionComponent _percetionComp;
        private Targeter _targeter;

        private void Awake()
        {
            _percetionComp = GetComponent<PerceptionComponent>();
            _targeter = GetComponent<Targeter>();
        }

        private void FixedUpdate()
        {
            _percetionComp.Actualize();
        }
    }
}
