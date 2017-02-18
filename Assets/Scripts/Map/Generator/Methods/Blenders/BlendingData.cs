using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.Generation
{
    public abstract class BlendingData : GenerationData
    {
        public abstract GenerationData GetFirstSource();
        public abstract GenerationData GetSecondSource();

        public uint GetConstructionLevel()
        {
            if (GetFirstSource().GetLevel() > GetSecondSource().GetLevel())
            {
                return GetFirstSource().GetLevel();
            }

            return GetSecondSource().GetLevel();
        }

        public int GetSmallestGridNum()
        {
            if (GetFirstSource().GetLevel() > GetSecondSource().GetLevel())
            {
                return 0;
            }

            return 1;
        }
    }
}
