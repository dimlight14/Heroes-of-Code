using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroesOfCode.Units;

namespace HeroesOfCode.Battle
{
    public class PlayerInteractedWithUnitEvent : CustomEvent
    {
        public UnitStackBase TargetUnit;
    }
}
