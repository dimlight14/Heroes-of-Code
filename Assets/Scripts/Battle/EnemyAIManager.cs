using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.Units;
using System;

namespace HeroesOfCode.Battle
{
    public class EnemyAIManager
    {
        private Dictionary<EnemyStrategyId, IEnemyStrategy> enemyStrategiesPool = new Dictionary<EnemyStrategyId, IEnemyStrategy>();
        private const float TurnExecutionDelay = 1;
        private UnitStackBase activeUnit;
        private BattleManager battleManager;
        private IEnemyStrategy currentStrategy;

        public void EnemyTurn(UnitStackBase activeUnit, BattleManager battleManager) {
            this.activeUnit = activeUnit;
            this.battleManager = battleManager;

            ActivateAppropriateStrategy(activeUnit.UnitType);
        }

        private void ActivateAppropriateStrategy(UnitType unitType) {
            switch (unitType) {
                case UnitType.SpecialCaster:
                    if (!enemyStrategiesPool.ContainsKey(EnemyStrategyId.TryToBuffPyro)) {
                        enemyStrategiesPool.Add(EnemyStrategyId.TryToBuffPyro, new SpecialCasterStrat());
                    }
                    currentStrategy = enemyStrategiesPool[EnemyStrategyId.TryToBuffPyro];
                    break;
                case UnitType.Pyromaniac:
                    if (!enemyStrategiesPool.ContainsKey(EnemyStrategyId.UsePyro)) {
                        enemyStrategiesPool.Add(EnemyStrategyId.UsePyro, new PyroStrategy());
                    }
                    currentStrategy = enemyStrategiesPool[EnemyStrategyId.UsePyro];
                    break;
                case UnitType.Defender:
                    if (!enemyStrategiesPool.ContainsKey(EnemyStrategyId.DefendThenAttackRandom)) {
                        enemyStrategiesPool.Add(EnemyStrategyId.DefendThenAttackRandom, new DefenderStrategy());
                    }
                    currentStrategy = enemyStrategiesPool[EnemyStrategyId.DefendThenAttackRandom];
                    break;
                case UnitType.Assassin:
                    if (!enemyStrategiesPool.ContainsKey(EnemyStrategyId.AttackMostDangerous)) {
                        enemyStrategiesPool.Add(EnemyStrategyId.AttackMostDangerous, new AssassinStrategy());
                    }
                    currentStrategy = enemyStrategiesPool[EnemyStrategyId.AttackMostDangerous];
                    break;
                default:
                    if (!enemyStrategiesPool.ContainsKey(EnemyStrategyId.AttackRandom)) {
                        enemyStrategiesPool.Add(EnemyStrategyId.AttackRandom, new AttackRandom());
                    }
                    currentStrategy = enemyStrategiesPool[EnemyStrategyId.AttackRandom];
                    break;
            }

            battleManager.StartCoroutine(DelayTurn(currentStrategy));
        }
        private IEnumerator DelayTurn(IEnemyStrategy currentStrategy) {
            yield return new WaitForSeconds(TurnExecutionDelay);
            currentStrategy.TakeTurn(activeUnit, battleManager);
        }

        public void Clear() {
            activeUnit = null;
            if (currentStrategy != null) battleManager.StopCoroutine(DelayTurn(currentStrategy));
        }
    }
}
