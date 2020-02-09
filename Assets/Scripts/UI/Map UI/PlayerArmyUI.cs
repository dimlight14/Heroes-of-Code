using System;
using System.Collections;
using System.Collections.Generic;
using HeroesOfCode.Units;
using UnityEngine;

namespace HeroesOfCode.UI
{
    public class PlayerArmyUI : MonoBehaviour
    {
        [SerializeField] private List<MapUIUnit> uintSlots = null;
        [SerializeField] private GameObject plaque = null;
        [SerializeField] private UnitSettingsSet unitSettingsSet = null;

        private Dictionary<UnitType, UnitSettings> unitSettingsMap;
        private Army playerArmy;

        private void Awake() {
            EventBus.Subscribe<BattleInitiatedEvent>(OnBattleInitiated);
            EventBus.Subscribe<BattleEndedEvent>(OnBattleEnded);
            EventBus.Subscribe<RegisterPlayerEvent>(OnPlayerRegister);

            unitSettingsMap = unitSettingsSet.unitSettingsMap;
        }

        private void OnPlayerRegister(RegisterPlayerEvent customEvent) {
            playerArmy = customEvent.PlayerArmy;
            DisplayArmy(playerArmy);
        }

        private void OnBattleEnded(BattleEndedEvent customEvent) {
            DisplayArmy(customEvent.playerArmy);
        }
        private void OnBattleInitiated(BattleInitiatedEvent customEvent) {
            HideMapUI();
        }
        private void HideMapUI() {
            plaque.SetActive(false);
        }

        public void DisplayArmy(Army playerArmy) {
            plaque.SetActive(true);
            foreach (MapUIUnit unitSlot in uintSlots) {
                unitSlot.gameObject.SetActive(false);
            }
            for (int i = 0; i < playerArmy.unitStacks.Count; i++) {
                UnitStackRepresentation unit = playerArmy.unitStacks[i];
                DisplayUnit(unit, i);
            }
        }
        private void DisplayUnit(UnitStackRepresentation unit, int position) {
            UnitSettings settings = unitSettingsMap[unit.type];

            uintSlots[position].gameObject.SetActive(true);
            uintSlots[position].SetInformation(
                "Имя   " + settings.baseData.unitName,
                "Количество " + unit.numberOfUnits,
                "Здоровье " + settings.baseData.maxHealth,
                "Урон " + settings.baseData.damage,
                "Инициатива " + settings.baseData.initiative
            );
        }
    }
}
