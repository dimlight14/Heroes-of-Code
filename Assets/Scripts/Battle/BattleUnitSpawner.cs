using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.UI;
using HeroesOfCode.Units;

namespace HeroesOfCode.Battle
{
    public class BattleUnitSpawner : MonoBehaviour
    {
        [SerializeField] private UnitStackFactory unitFactory = null;
        [SerializeField] private List<GameObject> battlefieldPlayerSlots = new List<GameObject>();
        [SerializeField] private List<GameObject> battlefieldEnemySlots = new List<GameObject>();
        [SerializeField] private BattleUIManager battleUI = null;

        private Army playerArmy;
        private Army enemyArmy;

        public List<UnitStackBase> SpawnPlayerUnits(Army playerArmy) {
            this.playerArmy = playerArmy;
            List<UnitStackBase> returnList = new List<UnitStackBase>();
            for (int i = 0; i < playerArmy.unitStacks.Count; i++) {
                UnitStackBase newUnit = unitFactory.Create(playerArmy.unitStacks[i], true);
                PlacePlayerUnit(newUnit.gameObject, i);
                battleUI.SetNumberOfUnitsPlaque(newUnit);
                returnList.Add(newUnit);
            }
            return returnList;
        }
        public List<UnitStackBase> SpawnEnemyUnits(Army enemyArmy) {
            this.enemyArmy = enemyArmy;
            List<UnitStackBase> returnList = new List<UnitStackBase>();
            for (int i = 0; i < enemyArmy.unitStacks.Count; i++) {
                UnitStackBase newUnit = unitFactory.Create(enemyArmy.unitStacks[i], false);
                PlaceEnemyUnit(newUnit.gameObject, i);
                battleUI.SetNumberOfUnitsPlaque(newUnit);
                returnList.Add(newUnit);
            }
            return returnList;
        }
        private void PlacePlayerUnit(GameObject unit, int position) {
            if (battlefieldPlayerSlots.Count < position + 1) {
                Debug.LogError($"Unit position {position} is not supported.");
                return;
            }

            unit.transform.position = battlefieldPlayerSlots[position].transform.position;
        }
        private void PlaceEnemyUnit(GameObject unit, int position) {
            if (battlefieldEnemySlots.Count < position + 1) {
                Debug.LogError($"Unit position {position} is not supported.");
                return;
            }

            unit.transform.position = battlefieldEnemySlots[position].transform.position;
        }
    }
}
