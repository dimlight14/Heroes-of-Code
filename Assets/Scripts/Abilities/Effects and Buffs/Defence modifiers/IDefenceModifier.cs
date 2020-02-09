using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroesOfCode.Units;

namespace HeroesOfCode.Abilities
{
    public interface IDefenceModifier
    {
        int ModifyDamageTaken(int initialDamge, UnitStackBase source, DamageType damageType);
    }
}
