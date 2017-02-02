using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure
{
    public class RelativeCoords
    {
        public Coords OriginCoords;

        public Coords VectorToOrigin;

        public int Distance
        {
            get
            {
                return Math.Abs(VectorToOrigin.abs) 
                    + Math.Abs(VectorToOrigin.ord);
            }
        }

        public RelativeCoords(Coords originCoords, Coords positionCoords)
        {
            OriginCoords = originCoords;

            Compute(positionCoords);
        }

        public void Compute(Coords positionCoords)
        {
            VectorToOrigin.abs = OriginCoords.abs - positionCoords.abs;
            VectorToOrigin.ord = OriginCoords.ord - positionCoords.ord;
        }
    }
}