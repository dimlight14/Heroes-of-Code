using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.Abilities;
using HeroesOfCode.Units;

namespace HeroesOfCode.Battle
{
    public class HealThenAttackRandom : IEnemyStrategy
    {
        private HealAbility healAbility;
        private BattleManager battleManager;
        private UnitStackBase actingUnit;

        public void TakeTurn(UnitStackBase unit, BattleManager battleManager) {
            this.battleManager = battleManager;
            actingUnit = unit;
            healAbility = unit.GetComponentInChildren<HealAbility>();
            if (healAbility == null) {
                Debug.LogError($"Unit doesn't have healing ability. Enemy strategy: HealThenAttackRandom. Enemy: {unit}.");
                AttackRandom();
            }
            else {
                TryToHeal();
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
        private void TryToHeal() {
            if (!healAbility.CanBeUsed()) {
                AttackRandom();
                return;
            }

            List<UnitStackBase> targetableUnits = new List<UnitStackBase>();
            foreach (var unit in battleManager.enemyUnitsInBattle) {
                if (healAbility.UnitIsViableTarget(unit)) targetableUnits.Add(unit);
            }
            if (targetableUnits.Count == 0) {
                AttackRandom();
            }
            else {
                int chance = UnityEngine.Random.Range(0, targetableUnits.Count);
                healAbility.GetActiatedByAi(targetableUnits[chance], battleManager);
                battleManager.EndTurn();
            }
        }
    }
}
