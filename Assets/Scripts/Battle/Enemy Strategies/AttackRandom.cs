using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.Units;
using HeroesOfCode.Abilities;

namespace HeroesOfCode.Battle
{
    public class AttackRandom : IEnemyStrategy
    {
        private BattleManager battleManager;
        private UnitStackBase actingUnit;

        public void TakeTurn(UnitStackBase unit, BattleManager battleManager) {
            this.battleManager = battleManager;
            this.actingUnit = unit;
            AttackRandomly();
        }

        private void AttackRandomly() {
            List<UnitStackBase> targetableUnits = new List<UnitStackBase>();
            foreach (var unit in battleManager.playerUnitsInBattle) {
                if (unit.IsTargetableByAttack()) targetableUnits.Add(unit);
            }
            if (targetableUnits.Count == 0) {
                battleManager.SkipTurn();
            }
            else {
                int chance = UnityEngine.Random.Range(0, targetableUnits.Count);
                targetableUnits[chance].GetDamaged(actingUnit.GetAttackDamage(), actingUnit, DamageType.Physical);
                battleManager.EndTurn();
            }
        }
    }
}
