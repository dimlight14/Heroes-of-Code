using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.UI;
using HeroesOfCode.Units;
using HeroesOfCode.Abilities;

namespace HeroesOfCode.Battle
{
    public class PlayerTurnManager : MonoBehaviour
    {
        private BattleManager battleManager;
        private IActiveAbility currentlySelectedAbility;
        private bool playerTurn = false;
        private UnitStackBase activeUnit;

        private void Awake() {
            EventBus.Subscribe<AbilityDeselectedEvent>(OnAbilityDeselected);
            EventBus.Subscribe<AbilitySelectedEvent>(OnAbilitySelected);
            EventBus.Subscribe<PlayerInteractedWithUnitEvent>(OnInteractionWithUnit);
        }

        public void StartTurn(UnitStackBase activeUnit, BattleManager battleManager) {
            this.battleManager = battleManager;
            this.activeUnit = activeUnit;
        }

        private void OnInteractionWithUnit(PlayerInteractedWithUnitEvent customEvent) {
            if (customEvent.TargetUnit == activeUnit) return;

            if (currentlySelectedAbility == null) {
                if (!customEvent.TargetUnit.PlayerUnit) {
                    AttackUnit(customEvent.TargetUnit);
                }
            }
            else {
                if (currentlySelectedAbility.UnitIsViableTarget(customEvent.TargetUnit)) {
                    currentlySelectedAbility.GetActivated(customEvent.TargetUnit, battleManager);
                    EventBus.FireEvent<AbilityDeselectedEvent>();
                    playerTurn = false;
                    battleManager.EndTurn();
                }
            }
        }
        private void OnAbilitySelected(AbilitySelectedEvent customEvent) {
            currentlySelectedAbility = customEvent.Ability;
            playerTurn = true;
        }
        private void OnAbilityDeselected(AbilityDeselectedEvent customEvent) {
            currentlySelectedAbility = null;
        }
        private void AttackUnit(UnitStackBase unit) {
            if (!unit.IsTargetableByAttack()) return;

            int damage = activeUnit.GetAttackDamage();
            unit.GetDamaged(damage, activeUnit, DamageType.Physical);
            playerTurn = false;
            battleManager.EndTurn();
        }

        private void Update() {
            if (playerTurn && currentlySelectedAbility != null) {
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    EventBus.FireEvent<AbilityDeselectedEvent>();
                }
            }
        }

        public void Clear() {
            activeUnit = null;
            playerTurn = false;
            currentlySelectedAbility = null;
        }
    }
}
