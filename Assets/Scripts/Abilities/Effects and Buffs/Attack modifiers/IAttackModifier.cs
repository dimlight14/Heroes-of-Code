using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesOfCode.Abilities
{
    public interface IAttackModifier
    {
        int ModifyDamage(int initialDamge);
    }
}
