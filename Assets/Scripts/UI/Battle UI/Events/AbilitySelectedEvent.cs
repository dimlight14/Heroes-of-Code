using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.Abilities;

namespace HeroesOfCode.UI
{
    public class AbilitySelectedEvent : CustomEvent
    {
        public IActiveAbility Ability;
    }
}
