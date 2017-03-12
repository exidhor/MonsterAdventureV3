using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    public class IntelligenceEngine : MonoSingleton<IntelligenceEngine>
    {
        private MappedList<int, IntelligenceComponent> _componentTable;

        private void Awake()
        {
            _componentTable = new MappedList<int, IntelligenceComponent>();
        }

        private void FixedUpdate()
        {
            PreUpdate();

            PostUpdate(Time.fixedDeltaTime);
        }

        private void PreUpdate()
        {
            for (int i = 0; i < _componentTable.Count; i++)
            {
                _componentTable.GetByIndex(i).PreUpdate();
            }
        }

        private void PostUpdate(float deltaTime)
        {
            for (int i = 0; i < _componentTable.Count; i++)
            {
                _componentTable.GetByIndex(i).PostUpdate(deltaTime);
            }
        }

        public void Register(IntelligenceComponent component)
        {
            _componentTable.Add(component.GetInstanceID(), component);
        }

        public void Unregister(IntelligenceComponent component)
        {
            _componentTable.RemoveFromKey(component.GetInstanceID());
        }
    }
}