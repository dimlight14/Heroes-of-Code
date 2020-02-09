using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.Units;
using HeroesOfCode.Abilities;

namespace HeroesOfCode.Battle
{
    public class AssassinStrategy : IEnemyStrategy
    {
        private UnitStackBase actingUnit;
        private BattleManager battleManager;

        public void TakeTurn(UnitStackBase actingUnit, BattleManager battleManager) {
            this.actingUnit = actingUnit;
            this.battleManager = battleManager;

            List<UnitStackBase> targetableUnits = new List<UnitStackBase>();
            foreach (UnitStackBase unit in battleManager.playerUnitsInBattle) {
                if (unit.IsTargetableByAttack()) {
                    targetableUnits.Add(unit);
                }
            }

            if (targetableUnits.Count == 0) {
                battleManager.SkipTurn();
                return;
            }

            UnitStackBase mostDangerousUnit = targetableUnits[0];
            foreach (UnitStackBase unit in targetableUnits) {
                if (unit.GetAttackDamage() > mostDangerousUnit.GetAttackDamage()) {
                    mostDangerousUnit = unit;
                }
            }
            AttackUnit(mostDangerousUnit);
        }

        private void AttackUnit(UnitStackBase targetUnit) {
            targetUnit.GetDamaged(actingUnit.GetAttackDamage(), actingUnit, DamageType.Physical);
            battleManager.EndTurn();
        }
    }
}
