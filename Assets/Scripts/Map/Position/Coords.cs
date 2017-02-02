using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure
{
    public struct Coords
    {
        public int abs;
        public int ord;

        public Coords(int abs, int ord)
        {
            this.abs = abs;
            this.ord = ord;
        }

        public bool IsNeighbour(Coords coords)
        {
            int sum = Math.Abs(abs - coords.abs) + Math.Abs(ord - coords.ord);

            return sum <= 1;
        }

        public RelativeCoords GetRelativeCoords(Coords origin)
        {
            return new RelativeCoords(this, origin);
        }

        public override string ToString()
        {
            return "(" + abs + "," + ord +")";
        }

        public static bool operator ==(Coords c0, Coords c1)
        {
            return c0.abs == c1.abs 
                && c0.ord == c1.ord;
        }

        public static bool operator !=(Coords c0, Coords c1)
        {
            return !(c0 == c1);
        }
    }
}
