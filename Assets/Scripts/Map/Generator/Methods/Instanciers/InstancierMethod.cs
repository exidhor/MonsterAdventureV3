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

        public InstancierMethod(InstancierData data, GenerationTable generationTable, SectorManager sectorManager)
            : base(data, generationTable, GenerationType.Instancier, data.dataSource.GetLevel())
        {
            _sectorManager = sectorManager;
        }

        protected override void FillGenerationGrid(GenerationGrid generationGrid)
        {
            // todo
        }
    }
}
