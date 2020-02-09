using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace HeroesOfCode.UI
{
    public class OnScreenHints : MonoBehaviour
    {
        [SerializeField] private Text hintsText = null;
        private StringBuilder stringBuilder = new StringBuilder();

        private void Awake() {
            EventBus.Subscribe<BattleInitiatedEvent>(OnBattleInitiated);
            EventBus.Subscribe<AbilityDeselectedEvent>(OnAbilityDeselected);
            EventBus.Subscribe<AbilitySelectedEvent>(OnAbilitySelected);
            EventBus.Subscribe<BattleEndedEvent>(OnBattleEnded);
            gameObject.SetActive(false);
        }

        private void OnBattleEnded(BattleEndedEvent customEvent) {
            gameObject.SetActive(false);
        }

        private void OnAbilitySelected(AbilitySelectedEvent customEvent) {
            DisplayAbilityHints();
        }

        private void OnAbilityDeselected(AbilityDeselectedEvent customEvent) {
            DisplayGeneralHints();
        }

        private void OnBattleInitiated(BattleInitiatedEvent customEvent) {
            DisplayGeneralHints();
        }

        private void DisplayGeneralHints() {
            gameObject.SetActive(true);
            stringBuilder.Clear();
            stringBuilder.AppendLine("Правая кнопка мыши:");
            stringBuilder.AppendLine("                     атаковать отряд");
            stringBuilder.AppendLine("Левая кнопка мыши:");
            stringBuilder.AppendLine("	     просмотреть отряд");
            stringBuilder.AppendLine("	     выбрать способность");
            hintsText.text = stringBuilder.ToString();
        }
        private void DisplayAbilityHints() {
            stringBuilder.Clear();
            stringBuilder.AppendLine("Правая кнопка мыши:");
            stringBuilder.AppendLine("            использовать способность");
            stringBuilder.AppendLine("Escape:");
            stringBuilder.AppendLine("            отменить способность");
            hintsText.text = stringBuilder.ToString();
        }
    }
}
