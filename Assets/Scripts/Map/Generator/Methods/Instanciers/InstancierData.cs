using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.Generation
{
    public class InstancierData : GenerationData
    {
        public GroupingData dataSource;

        public List<InstancierValue> InstancierValues;

        private Dictionary<int, InstancierValue> _sortedInstancierValues;

        private Map _map;
        private PoolAllocatorTable _poolAllocatorTable;

        protected override void AwakeContent()
        {
            _sortedInstancierValues = new Dictionary<int, InstancierValue>();

            for (int i = 0; i < InstancierValues.Count; i++)
            {
                _sortedInstancierValues.Add(InstancierValues[i].Value, 
                    InstancierValues[i]);
            }
        }

        protected override void StartContent()
        {
            _map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();

            _poolAllocatorTable =
                GameObject.FindGameObjectWithTag("PoolAllocatorTable").GetComponent<PoolAllocatorTable>();
        }

        protected override GenerationMethod ConstructGenerationMethod(GenerationTable generationTable)
        {
            return new InstancierMethod(this, generationTable, _map.SectorManager, _poolAllocatorTable);
        }

        public Dictionary<int, InstancierValue> GetDictionary()
        {
            return _sortedInstancierValues;
        }
    }
}
