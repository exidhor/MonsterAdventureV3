using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    public class AIEngine : MonoSingleton<AIEngine>
    {
        private MappedList<int, AIComponent> _componentTable;

        private void Awake()
        {
            _componentTable = new MappedList<int, AIComponent>();
        }

        private void FixedUpdate()
        {
            ActualizeSteerings();

            ActualizeKinematics(Time.fixedDeltaTime);
        }

        private void ActualizeSteerings()
        {
            for (int i = 0; i < _componentTable.Count; i++)
            {
                _componentTable.GetByIndex(i).ActualizeSteerings();
            }
        }

        private void ActualizeKinematics(float deltaTime)
        {
            for (int i = 0; i < _componentTable.Count; i++)
            {
                _componentTable.GetByIndex(i).ActualizeKinematic(deltaTime);
            }
        }

        public void Register(AIComponent component)
        {
            _componentTable.Add(component.GetInstanceID(), component);
        }

        public void Unregister(AIComponent component)
        {
            _componentTable.RemoveFromKey(component.GetInstanceID());
        }
    }
}