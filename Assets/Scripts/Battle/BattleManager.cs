using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.UI;
using HeroesOfCode.Units;

namespace HeroesOfCode.Battle
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private BattleUIManager battleUI = null;
        [SerializeField] private PlayerTurnManager playerTurnManager = null;
        [SerializeField] private BattleUnitSpawner unitSpawner = null;

        private Army playerArmy;
        private Army enemyArmy;
        [HideInInspector] public List<UnitStackBase> playerUnitsInBattle = new List<UnitStackBase>();
        [HideInInspector] public List<UnitStackBase> enemyUnitsInBattle = new List<UnitStackBase>();
        private List<UnitStackBase> deadUnits = new List<UnitStackBase>();

        private const float EndBattleDelay = 1.5f;
        private InitiativeResolver initiativeResolver;
        private EnemyAIManager enemyAI;
        private UnitStackBase activeUnit;
        private bool battleLost = false;

        private void Awake() {
            initiativeResolver = new InitiativeResolver();
            enemyAI = new EnemyAIManager();
            EventBus.Subscribe<BattleInitiatedEvent>(OnBattleInitiated);
            EventBus.Subscribe<UnitDiedEvent>(OnUnitDied);
        }

        private void OnBattleInitiated(BattleInitiatedEvent customEvent) {
            StartBattle(customEvent.PlayerArmy, customEvent.EnemyArmy);
        }

        private void StartBattle(Army playerArmy, Army enemyArmy) {
            this.playerArmy = playerArmy;
            this.enemyArmy = enemyArmy;

            playerUnitsInBattle = unitSpawner.SpawnPlayerUnits(playerArmy);
            enemyUnitsInBattle = unitSpawner.SpawnEnemyUnits(enemyArmy);
            battleLost = false;
            SetUnitAsActive(initiativeResolver.ResolveInitiative(playerUnitsInBattle, enemyUnitsInBattle));
        }
        public void SkipTurn() {
            EndTurn();
        }
        public void EndTurn() {
            if (playerUnitsInBattle.Count == 0) {
                LoseBattle();
                return;
            }
            else if (enemyUnitsInBattle.Count == 0) {
                WinBattle();
                return;
            }
            activeUnit.unitSelection.SetSelectionImage(UnitSelection.SelectionType.None);
            SetUnitAsActive(initiativeResolver.ResolveInitiative(playerUnitsInBattle, enemyUnitsInBattle));
        }
        private void SetUnitAsActive(UnitStackBase unit) {
            activeUnit = unit;
            activeUnit.unitSelection.SetSelectionImage(UnitSelection.SelectionType.Active);
            battleUI.PrintActiveUnitStats(activeUnit);
            if (activeUnit.PlayerUnit) {
                playerTurnManager.StartTurn(activeUnit, this);
            }
            else {
                enemyAI.EnemyTurn(activeUnit, this);
            }
        }
        private void OnUnitDied(UnitDiedEvent customEvent) {
            UnitStackBase unit = customEvent.Unit;
            if (playerUnitsInBattle.Contains(unit)) {
                playerUnitsInBattle.Remove(unit);
            }
            else if (enemyUnitsInBattle.Contains(unit)) {
                enemyUnitsInBattle.Remove(unit);
            }
            else return;

            unit.gameObject.SetActive(false);
            deadUnits.Add(unit);
            battleUI.DeselectCurrentTarget();
        }

        #region Battle End
        private void LoseBattle() {
            Debug.Log("You lost the battle");
            battleUI.LoseBattle();
            battleLost = true;
        }
        private void WinBattle() {
            Debug.Log("You won the battle");
            battleUI.WinBattle();
            StopCoroutine("EndBattleAndSaveResults");
            StartCoroutine("EndBattleAndSaveResults");
        }
        private IEnumerator EndBattleAndSaveResults() {
            yield return new WaitForSeconds(EndBattleDelay);

            playerArmy.unitStacks.Clear();
            foreach (UnitStackBase unit in playerUnitsInBattle) {
                playerArmy.unitStacks.Add(new UnitStackRepresentation() {
                    numberOfUnits = unit.NumberOfUnits,
                    type = unit.UnitType
                });
            }
            ClearAll();
            battleUI.ClearAll();
            Debug.Log("Battle end");
            EventBus.FireEvent<BattleEndedEvent>(new BattleEndedEvent { playerArmy = this.playerArmy, enemyArmy = this.enemyArmy });
        }
        private void RestartBattle() {
            ClearAll();
            EventBus.FireEvent<BattleInitiatedEvent>(new BattleInitiatedEvent() { EnemyArmy = enemyArmy, PlayerArmy = playerArmy });
        }
        private void ClearAll() {
            foreach (UnitStackBase unit in deadUnits) {
                unit.DestroySelf();
            }
            foreach (UnitStackBase unit in playerUnitsInBattle) {
                unit.DestroySelf();
            }
            foreach (UnitStackBase unit in enemyUnitsInBattle) {
                unit.DestroySelf();
            }

            deadUnits.Clear();
            playerUnitsInBattle.Clear();
            enemyUnitsInBattle.Clear();
            enemyAI.Clear();
            playerTurnManager.Clear();
            initiativeResolver.Clear();
        }
        #endregion
        private void Update() {
            if (battleLost) {
                if (Input.GetKeyDown(KeyCode.Space)) {
                    RestartBattle();
                }
            }
        }
    }
}