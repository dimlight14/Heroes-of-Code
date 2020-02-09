using System.Collections;
using System.Collections.Generic;
using HeroesOfCode.Units;
using UnityEngine;

using HeroesOfCode.Abilities;

namespace HeroesOfCode.Battle
{
    public class SpecialCasterStrat : IEnemyStrategy
    {
        private UnitStackBase actingUnit;
        private BattleManager battleManager;
        private BuffDamageAbility ability;

        public void TakeTurn(UnitStackBase actingUnit, BattleManager battleManager) {
            this.actingUnit = actingUnit;
            this.battleManager = battleManager;
            this.ability = actingUnit.GetComponentInChildren<BuffDamageAbility>();

            if (ability == null) {
                Debug.LogError($"Unit doesn't have buffing ability. Enemy strategy: SpecialCasterStrat. Enemy: {actingUnit}.");
                AttackRandom();
                return;
            }
            if (ability.CanBeUsed()) {
                TryToBuffPyro();
            }
            else {
                AttackRandom();
            }
        }

        private void TryToBuffPyro() {
            List<UnitStackBase> abilitytargets = new List<UnitStackBase>();
            foreach (UnitStackBase unit in battleManager.enemyUnitsInBattle) {
                if (ability.UnitIsViableTarget(unit)) {
                    abilitytargets.Add(unit);
                }
            }

            if (abilitytargets.Count == 0) {
                AttackRandom();
                return;
            }

            UnitStackBase finalAbilityTarget = abilitytargets[0];
            foreach (UnitStackBase unit in abilitytargets) {
                if (unit.UnitType == UnitType.Pyromaniac) {
                    finalAbilityTarget = unit;
                }
            }
            ability.GetActiatedByAi(finalAbilityTarget, battleManager);
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
