using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.Abilities;
using HeroesOfCode.Units;

namespace HeroesOfCode.Battle
{
    public class PyroStrategy : IEnemyStrategy
    {
        private UnitStackBase actingUnit;
        private BattleManager battleManager;
        private SmallmageddonAbility ability;

        public void TakeTurn(UnitStackBase actingUnit, BattleManager battleManager) {
            this.actingUnit = actingUnit;
            this.battleManager = battleManager;
            this.ability = actingUnit.GetComponentInChildren<SmallmageddonAbility>();

            if (ability == null) {
                Debug.LogError($"Unit doesn't have smallmageddon ability. Enemy strategy: PyroStrategy. Enemy: {actingUnit}.");
                AttackRandom();
                return;
            }

            if (ability.CanBeUsed()) {
                ability.GetActiatedByAi(actingUnit, battleManager);
                battleManager.EndTurn();
            }
            else {
                AttackRandom();
            }
        }

        private void AttackRandom() {
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
