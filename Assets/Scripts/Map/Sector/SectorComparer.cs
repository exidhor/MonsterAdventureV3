using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure
{
    public class SectorComparer : IComparer<Sector>
    {
        public int Compare(Sector x, Sector y)
        {
            if (x.GetCoords().ord != y.GetCoords().ord)
            {
                return x.GetCoords().ord.CompareTo(y.GetCoords().ord);
            }

            return x.GetCoords().abs.CompareTo(y.GetCoords().abs);
        }
    }
}
