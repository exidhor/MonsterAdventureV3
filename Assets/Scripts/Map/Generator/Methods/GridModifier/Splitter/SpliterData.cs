using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.Generation
{
    public class SpliterData : GridModifierData
    {
        public GenerationData dataSource;
        [Range(1, 10)] public uint splitLevel = 1;

        protected override void AwakeContent()
        {
            // nothing
        }

        protected override void StartContent()
        {
            //nothing
        }

        protected override GenerationMethod ConstructGenerationMethod(GenerationTable generationTable)
        {
            return new Splitter(this, generationTable);
        }

        public override GenerationData GetSource()
        {
            return dataSource;
        }

        public uint GetModifiedLevel()
        {
            return splitLevel + dataSource.GetLevel();
        }
    }
}
