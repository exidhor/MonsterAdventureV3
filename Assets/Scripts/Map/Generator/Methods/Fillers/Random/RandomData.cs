using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.Generation
{
    public class RandomData : FillerData
    {
        protected override void AwakeContent()
        {
            // nothing
        }

        protected override void StartContent()
        {

        }

        protected override GenerationMethod ConstructGenerationMethod(GenerationTable generationTable)
        {
            return new RandomSpread(this, generationTable);
        }
    }
}
