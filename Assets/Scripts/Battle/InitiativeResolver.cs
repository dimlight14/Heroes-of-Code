using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.Units;
using System;

namespace HeroesOfCode.Battle
{
    public class InitiativeResolver
    {
        private List<UnitStackBase> initiativeBlackList = new List<UnitStackBase>();
        private List<UnitStackBase> nextHighestInitiative = new List<UnitStackBase>();


        public UnitStackBase ResolveInitiative(List<UnitStackBase> playerUnitsInBattle, List<UnitStackBase> enemyUnitsInBattle) {
            nextHighestInitiative.Clear();

            CheckUnitsInArmy(playerUnitsInBattle);
            CheckUnitsInArmy(enemyUnitsInBattle);

            if (nextHighestInitiative.Count == 0) {
                initiativeBlackList.Clear();
                CheckUnitsInArmy(playerUnitsInBattle);
                CheckUnitsInArmy(enemyUnitsInBattle);
            }

            int chance = UnityEngine.Random.Range(0, nextHighestInitiative.Count);
            initiativeBlackList.Add(nextHighestInitiative[chance]);
            return nextHighestInitiative[chance];
        }
        private void CheckUnitsInArmy(List<UnitStackBase> army) {
            foreach (UnitStackBase unit in army) {
                if (initiativeBlackList.Contains(unit)) continue;

                if (nextHighestInitiative.Count == 0) {
                    nextHighestInitiative.Add(unit);
                }
                else {
                    if (unit.Initiative == nextHighestInitiative[0].Initiative) {
                        nextHighestInitiative.Add(unit);
                    }
                    else if (unit.Initiative > nextHighestInitiative[0].Initiative) {
                        nextHighestInitiative.Clear();
                        nextHighestInitiative.Add(unit);
                    }
                }
            }
        }

        public void Clear() {
            initiativeBlackList.Clear();
            nextHighestInitiative.Clear();
        }
    }
}
