using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.Units;

namespace HeroesOfCode.UI
{
    public class UnitClickedEvent : CustomEvent
    {
        public UnitStackBase Unit;
        public UnitSelection UnitSelection;
    }
}
