using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.UI;
using HeroesOfCode.Units;
using HeroesOfCode.Battle;

namespace HeroesOfCode.Abilities
{
    public interface IActiveAbility : IDisplayableIcon
    {
        void GetActiatedByAi(UnitStackBase targetUnit, BattleManager manager);
        void GetActivated(UnitStackBase targetUnit, BattleManager manager);
        bool UnitIsViableTarget(UnitStackBase targetUnit);
        bool CanBeUsed();
    }
}
