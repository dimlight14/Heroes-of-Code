using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.Abilities;
using HeroesOfCode.Units;

namespace HeroesOfCode.Battle
{
    public class DefenderStrategy : IEnemyStrategy
    {
        private UnitStackBase actingUnit;
        private BattleManager battleManager;
        private DefendAbility defendAbility;

        public void TakeTurn(UnitStackBase unit, BattleManager battleManager) {
            this.actingUnit = unit;
            this.battleManager = battleManager;
            defendAbility = actingUnit.GetComponentInChildren<DefendAbility>();
            if (defendAbility == null) {
                Debug.LogError($"Unit doesn't have defending ability. Enemy strategy: DefenderStrategy. Enemy: {unit}.");
                AttackRandom();
                return;
            }
            if (defendAbility.CanBeUsed()) {
                TryToUseDefend();
            }
            else {
                AttackRandom();
            }
        }

        private void TryToUseDefend() {
            List<UnitStackBase> abilityTargets = new List<UnitStackBase>();
            foreach (UnitStackBase unit in battleManager.enemyUnitsInBattle) {
                if (defendAbility.UnitIsViableTarget(unit)) {
                    abilityTargets.Add(unit);
                }
            }

            if (abilityTargets.Count == 0) {
                AttackRandom();
                return;
            }

            UnitStackBase mostDangerous = abilityTargets[0];
            foreach (UnitStackBase unit in abilityTargets) {
                if (unit.GetAttackDamage() > mostDangerous.GetAttackDamage()) {
                    mostDangerous = unit;
                }
            }
            defendAbility.GetActiatedByAi(mostDangerous, battleManager);
            battleManager.EndTurn();
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
