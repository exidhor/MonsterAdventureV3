using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.Generation
{
    public class InstancierData : GenerationData
    {
        public GenerationData dataSource;

        public List<InstancierValue> InstancierValues;

        private Dictionary<int, InstancierValue> _sortedInstancierValues;

        private Map _map;
        //private PoolTable _poolTable;

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

        }

        protected override GenerationMethod ConstructGenerationMethod(GenerationTable generationTable)
        {
            _map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();

            //_poolTable =
            //   GameObject.FindGameObjectWithTag("PoolAllocatorTable").GetComponent<PoolTable>();

            //return new InstancierMethod(this, generationTable, _map.SectorManager, _poolTable);
            return new InstancierMethod(this, generationTable, _map.SectorManager);
        }

        public Dictionary<int, InstancierValue> GetDictionary()
        {
            return _sortedInstancierValues;
        }
    }
}
