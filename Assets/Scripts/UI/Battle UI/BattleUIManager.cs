using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using HeroesOfCode.Units;
using HeroesOfCode.Abilities;

namespace HeroesOfCode.UI
{
    public class BattleUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject allUIConatiner = null;
        [SerializeField] private Text nameActiveText = null;
        [SerializeField] private Text numberActiveText = null;
        [SerializeField] private Text HPActiveText = null;
        [SerializeField] private Text currentHPActiveText = null;
        [SerializeField] private Text damageActiveText = null;
        [SerializeField] private Text initiativeActiveText = null;
        [SerializeField] private Text iconDescriptionActiveText = null;
        [SerializeField] private Text iconNameActiveText = null;
        [SerializeField] private IconController activeUnitAbilitySlot1 = null;
        [SerializeField] private IconController activeUnitAbilitySlot2 = null;
        [SerializeField] private IconController activeUnitAbilitySlot3 = null;
        [Space(15)]
        [SerializeField] private Text nameSelectedText = null;
        [SerializeField] private Text numberSelectedText = null;
        [SerializeField] private Text HPSelectedText = null;
        [SerializeField] private Text currentHPSelectedText = null;
        [SerializeField] private Text damageSelectedText = null;
        [SerializeField] private Text initiativeSelectedText = null;
        [SerializeField] private Text iconDescriptionSelectedText = null;
        [SerializeField] private Text iconNameSelectedText = null;
        [SerializeField] private IconController selectedUnitAbilitySlot1 = null;
        [SerializeField] private IconController selectedUnitAbilitySlot2 = null;
        [SerializeField] private IconController selectedUnitAbilitySlot3 = null;
        [Space(15)]
        [SerializeField] private GameObject numberOfUnitsPrefab = null;
        [Space(25)]
        [SerializeField] private Text battleLostText = null;
        [SerializeField] private Text battleWinText = null;
        [SerializeField] private Canvas mainCanvas = null;

        private Camera mainCamera;
        private UnitSelection savedSelection;

        private void Awake() {
            EventBus.Subscribe<UnitClickedEvent>(OnUnitClicked);
            EventBus.Subscribe<BattleInitiatedEvent>(OnBattleInitiated);
            EventBus.Subscribe<IconUnderMouseEvent>(OnIconUnderMouse);
            EventBus.Subscribe<MouseLeavesIconEvent>(OnMouseLevesIcon);
        }

        private void OnBattleInitiated(BattleInitiatedEvent customEvent) {
            allUIConatiner.SetActive(true);
            battleLostText.gameObject.SetActive(false);
            battleWinText.gameObject.SetActive(false);
            DeselectCurrentTarget();
            ClearActiveUnitStats();
        }

        private void Start() {
            mainCamera = Camera.main;
        }

        #region Units and stats
        public void PrintActiveUnitStats(UnitStackBase unit) {
            ClearActiveUnitStats();

            nameActiveText.gameObject.SetActive(true);
            numberActiveText.gameObject.SetActive(true);
            HPActiveText.gameObject.SetActive(true);
            currentHPActiveText.gameObject.SetActive(true);
            damageActiveText.gameObject.SetActive(true);
            initiativeActiveText.gameObject.SetActive(true);

            nameActiveText.text = unit.UnitName;
            numberActiveText.text = "Количество " + unit.NumberOfUnits;
            HPActiveText.text = "Max HP " + unit.MaxHealth;
            currentHPActiveText.text = "Current HP " + unit.TopUnitHealth;
            damageActiveText.text = "Урон " + unit.Damage;
            initiativeActiveText.text = "Инициатива " + unit.Initiative;

            List<IActiveAbility> abilities = new List<IActiveAbility>();
            abilities.AddRange(unit.GetComponentsInChildren<IActiveAbility>());
            foreach (IActiveAbility ability in abilities) {
                SetActiveUnitAbility(ability);
            }
            List<IDisplayIconWrapper> blackList = new List<IDisplayIconWrapper>();
            blackList.AddRange(abilities);

            List<IDisplayIconWrapper> icons = new List<IDisplayIconWrapper>();
            icons.AddRange(unit.GetComponentsInChildren<IDisplayIconWrapper>());
            foreach (IDisplayIconWrapper icon in icons) {
                if (blackList.Contains(icon)) continue;

                DisplayActiveUnitIcon(icon);
            }
        }
        private void ClearActiveUnitStats() {
            nameActiveText.gameObject.SetActive(false);
            numberActiveText.gameObject.SetActive(false);
            HPActiveText.gameObject.SetActive(false);
            currentHPActiveText.gameObject.SetActive(false);
            damageActiveText.gameObject.SetActive(false);
            initiativeActiveText.gameObject.SetActive(false);
            activeUnitAbilitySlot1.DisableSelf();
            activeUnitAbilitySlot2.DisableSelf();
            activeUnitAbilitySlot3.DisableSelf();
        }
        private void DisplayActiveUnitIcon(IDisplayIconWrapper icon) {
            if (!activeUnitAbilitySlot1.active) {
                activeUnitAbilitySlot1.SetIcon(icon);
            }
            else if (!activeUnitAbilitySlot2.active) {
                activeUnitAbilitySlot2.SetIcon(icon);
            }
            else if (!activeUnitAbilitySlot3.active) {
                activeUnitAbilitySlot3.SetIcon(icon);
            }
        }
        private void SetActiveUnitAbility(IActiveAbility ability) {
            if (!activeUnitAbilitySlot1.active) {
                activeUnitAbilitySlot1.SetIcon(ability);
                activeUnitAbilitySlot1.SetActiveAbility(ability);
            }
            else if (!activeUnitAbilitySlot2.active) {
                activeUnitAbilitySlot2.SetIcon(ability);
                activeUnitAbilitySlot2.SetActiveAbility(ability);
            }
            else if (!activeUnitAbilitySlot3.active) {
                activeUnitAbilitySlot3.SetIcon(ability);
                activeUnitAbilitySlot3.SetActiveAbility(ability);
            }
        }

        private void OnIconUnderMouse(IconUnderMouseEvent customEvent) {
            DisplayIconDescription(customEvent.Icon, customEvent.IconBelongsToActiveUnit);
        }
        private void OnMouseLevesIcon(MouseLeavesIconEvent customEvent) {
            ClearIconDescription(customEvent.IconBelongsToActiveUnit);
        }
        private void OnUnitClicked(UnitClickedEvent customEvent) {
            PrintSelectedUnitStats(customEvent.Unit);
            SaveSelectedTargetUnit(customEvent.UnitSelection);
        }

        private void DisplayIconDescription(IDisplayIconWrapper iconObject, bool activeUnit) {
            if (activeUnit) {
                iconDescriptionActiveText.text = iconObject.GetDescription();
                iconNameActiveText.text = iconObject.GetName();
            }
            else {
                iconDescriptionSelectedText.text = iconObject.GetDescription();
                iconNameSelectedText.text = iconObject.GetName();
            }
        }
        private void ClearIconDescription(bool activeUnit) {
            if (activeUnit) {
                iconDescriptionActiveText.text = "";
                iconNameActiveText.text = "";
            }
            else {
                iconDescriptionSelectedText.text = "";
                iconNameSelectedText.text = "";
            }
        }

        private void DisplaySelectedUnitIcon(IDisplayIconWrapper icon, int position) {
            switch (position) {
                case 0:
                    selectedUnitAbilitySlot1.SetIcon(icon);
                    break;
                case 1:
                    selectedUnitAbilitySlot2.SetIcon(icon);
                    break;
                case 2:
                    selectedUnitAbilitySlot3.SetIcon(icon);
                    break;
            }
        }

        private readonly Vector2 PlayerUnitPlaqueOffset = new Vector2(-100, 210);
        private readonly Vector2 EnemyUnitPlaqueOffset = new Vector2(100, 210);
        public void SetNumberOfUnitsPlaque(UnitStackBase newUnit) {
            Vector3 plaquePosition;
            if (newUnit.PlayerUnit) {
                plaquePosition = new Vector3(
                    mainCamera.WorldToScreenPoint(newUnit.transform.position).x + PlayerUnitPlaqueOffset.x,
                    mainCamera.WorldToScreenPoint(newUnit.transform.position).y + PlayerUnitPlaqueOffset.y,
                    0
                );
            }
            else {
                plaquePosition = new Vector3(
                    mainCamera.WorldToScreenPoint(newUnit.transform.position).x + EnemyUnitPlaqueOffset.x,
                    mainCamera.WorldToScreenPoint(newUnit.transform.position).y + EnemyUnitPlaqueOffset.y,
                    0
                );
            }
            GameObject newPlaque = Instantiate(numberOfUnitsPrefab, plaquePosition, Quaternion.identity, mainCanvas.transform);
            newUnit.SetNumberOfUnitsPlaque(newPlaque.GetComponentInChildren<Text>());
        }

        private void SaveSelectedTargetUnit(UnitSelection unitSelection) {
            if (savedSelection != null) savedSelection.SetSelectionImage(UnitSelection.SelectionType.None);
            savedSelection = unitSelection;
        }
        private void PrintSelectedUnitStats(UnitStackBase unit) {
            DeselectCurrentTarget();
            nameSelectedText.gameObject.SetActive(true);
            numberSelectedText.gameObject.SetActive(true);
            HPSelectedText.gameObject.SetActive(true);
            currentHPSelectedText.gameObject.SetActive(true);
            damageSelectedText.gameObject.SetActive(true);
            initiativeSelectedText.gameObject.SetActive(true);

            nameSelectedText.text = unit.UnitName;
            numberSelectedText.text = "Количество " + unit.NumberOfUnits;
            HPSelectedText.text = "Max HP " + unit.MaxHealth;
            currentHPSelectedText.text = "Current HP " + unit.TopUnitHealth;
            damageSelectedText.text = "Урон " + unit.Damage;
            initiativeSelectedText.text = "Инициатива " + unit.Initiative;

            List<IDisplayIconWrapper> icons = new List<IDisplayIconWrapper>();
            icons.AddRange(unit.GetComponentsInChildren<IDisplayIconWrapper>());
            for (int i = 0; i < icons.Count; i++) {
                IDisplayIconWrapper icon = icons[i];
                DisplaySelectedUnitIcon(icon, i);
            }
        }

        public void DeselectCurrentTarget() {
            if (savedSelection != null) savedSelection.SetSelectionImage(UnitSelection.SelectionType.None);
            nameSelectedText.gameObject.SetActive(false);
            numberSelectedText.gameObject.SetActive(false);
            HPSelectedText.gameObject.SetActive(false);
            currentHPSelectedText.gameObject.SetActive(false);
            damageSelectedText.gameObject.SetActive(false);
            initiativeSelectedText.gameObject.SetActive(false);

            selectedUnitAbilitySlot1.gameObject.SetActive(false);
            selectedUnitAbilitySlot2.gameObject.SetActive(false);
            selectedUnitAbilitySlot3.gameObject.SetActive(false);
            ClearIconDescription(false);
        }
        #endregion

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                DeselectCurrentTarget();
            }
        }

        public void LoseBattle() {
            DeselectCurrentTarget();
            ClearActiveUnitStats();
            battleLostText.gameObject.SetActive(true);
        }
        public void WinBattle() {
            DeselectCurrentTarget();
            ClearActiveUnitStats();
            battleWinText.gameObject.SetActive(true);
        }
        public void ClearAll() {
            battleWinText.gameObject.SetActive(false);
            allUIConatiner.SetActive(false);
        }
    }
}