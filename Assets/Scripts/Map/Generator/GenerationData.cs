using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.Generation
{
    public abstract class GenerationData : MonoBehaviour
    {
        public string name;
        //public uint level;

        private GenerationMethod _generationMethod;

        private void Awake()
        {
            AwakeContent();

            _generationMethod = null;
        }

        private void Start()
        {
            StartContent();
        }

        protected abstract void AwakeContent();

        protected abstract void StartContent();

        public GenerationMethod Construct(GenerationTable generationTable)
        {
            if (_generationMethod == null)
            {
                _generationMethod = ConstructGenerationMethod(generationTable);
            }

            return _generationMethod;
        }

        protected abstract GenerationMethod ConstructGenerationMethod(GenerationTable generationTable);

        public GenerationMethod GetGenerationMethod()
        {
            return _generationMethod;
        }

        public uint GetLevel()
        {
            return _generationMethod.GetLevel();
        }
    }
} 