using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace MonsterAdventure.AI
{
    public enum LocationType
    {
        StationaryLocation,
        TransformLocation
    }

    public abstract class Location
    {
        public abstract Vector2 GetPosition();

        public abstract LocationType GetLocationType();
    }
}
