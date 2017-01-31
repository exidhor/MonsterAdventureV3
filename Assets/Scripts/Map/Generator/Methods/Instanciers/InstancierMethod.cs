using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.Generation
{
    public class InstancierMethod : GenerationMethod
    {
        public InstancierMethod(InstancierData data, GenerationTable generationTable)
            : base(data, generationTable, GenerationType.Instancier, data.dataSource.GetLevel())
        {

        }

        protected override void FillGenerationGrid(GenerationGrid generationGrid)
        {
            throw new NotImplementedException();
        }
    }
}
