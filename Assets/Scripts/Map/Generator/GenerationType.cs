﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonsterAdventure.Generation
{
    public enum GenerationType
    {
        // Fillers
        Noise,
        Random,

        // Modifiers
        Grouping,
        Distance,
        Splitter,

        // Blenders
        Blend,

        // Instanciers
        Instancier    
    }
}
