using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.Generation
{
    public class InstancierMethod : GenerationMethod
    {
        private SectorManager _sectorManager;
        private PoolAllocatorTable _poolAllocatorTable;

        public InstancierMethod(InstancierData data, GenerationTable generationTable, SectorManager sectorManager, PoolAllocatorTable poolAllocatorTable)
            : base(data, generationTable, GenerationType.Instancier, data.dataSource.GetLevel())
        {
            _sectorManager = sectorManager;
            _poolAllocatorTable = poolAllocatorTable;

            ComputeGeneration();
        }

        protected override void FillGenerationGrid(GenerationGrid generationGrid)
        {
            CreatePools();

            Dictionary<int, InstancierValue> models = GetInstancierData().GetDictionary();
            InstancierValue instancierValue = null;
            GenerationGrid sourceGrid = GetInstancierData().dataSource.GetGenerationMethod().GetGrid();

            for (int i = 0; i < sourceGrid.lineSize; i++)
            {
                for (int j = 0; j < sourceGrid.lineSize; j++)
                {
                    if (models.TryGetValue(sourceGrid.Get(i, j).GetIntValue(), 
                        out instancierValue))
                    {
                        // create TracedObject
                        TracedObject tracedObject = _poolAllocatorTable.GetTracedObject(instancierValue);

                        tracedObject.Position = PositionFromGrid(i, j, sourceGrid);

                        _sectorManager.AddTracedObjectToSector(tracedObject);
                    }
                }
            }
        }

        private void CreatePools()
        {
            List<InstancierValue> instancierValues = GetInstancierData().InstancierValues;

            for (int i = 0; i < instancierValues.Count; i++)
            {
                _poolAllocatorTable.AddPoolAllocator(instancierValues[i]);
            }
        }

        public Vector2 PositionFromGrid(int x, int y, GenerationGrid grid)
        {
            float mapSize = _sectorManager.MapSize;

            float boxSize = mapSize/grid.lineSize;

            return new Vector2(x * boxSize + _sectorManager.MapOffset + boxSize / 2,
                y * boxSize + _sectorManager.MapOffset + boxSize / 2); 
        }

        private InstancierData GetInstancierData()
        {
            return (InstancierData) _generationData;
        }
    }
}
