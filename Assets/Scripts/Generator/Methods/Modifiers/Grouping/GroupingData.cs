﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.Generation
{
    public class GroupingData : ModifierData
    {
        public List<GroupingValue> groupingValues;

        protected override void AwakeContent()
        {
            // nothing
        }

        protected override GenerationMethod ConstructGenerationMethod(GenerationTable generationTable)
        {
            return new Grouping(this, generationTable);
        }
    }
}