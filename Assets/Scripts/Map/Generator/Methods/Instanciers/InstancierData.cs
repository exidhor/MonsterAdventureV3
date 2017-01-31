﻿using System;
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

        private Map _map;

        protected override void AwakeContent()
        {
            // nothing
        }

        protected override void StartContent()
        {
            _map = GameObject.FindGameObjectWithTag("Map").GetComponent<Map>();
        }

        protected override GenerationMethod ConstructGenerationMethod(GenerationTable generationTable)
        {
            return new InstancierMethod(this, generationTable, _map.SectorManager);
        }
    }
}
