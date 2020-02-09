using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroesOfCode.Units;

namespace HeroesOfCode.Battle
{
    public enum EnemyStrategyId
    {
        AttackRandom,
        HealThenAttackRandom,
        AttackMostDangerous,
        DefendThenAttackRandom,
        UsePyro,
        TryToBuffPyro
    }
    public interface IEnemyStrategy
    {
        void TakeTurn(UnitStackBase unit, BattleManager battleManager);
    }
}
