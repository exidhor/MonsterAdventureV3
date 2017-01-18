using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.Generation
{
    public class Generator : MonoBehaviour
    {
        public List<GenerationData> orderedGenerationsDatas;

        private GenerationTable _generationTable;

        private bool _isInitialized = false;

        private void Awake()
        {
            // nothing
        }

        public void Construct()
        {
            _generationTable = new GenerationTable();

            ConstructGenerationMethods();

            _isInitialized = true;
        }

        private void ConstructGenerationMethods()
        {
            for (int i = 0; i < orderedGenerationsDatas.Count; i++)
            {
                orderedGenerationsDatas[i].Construct(_generationTable);
            }
        }

        public List<GenerationMethod> GetGenerationMethods()
        {
            return _generationTable.GetAll();
        }

        public bool IsInitialized()
        {
            return _isInitialized;
        }
    }
}
