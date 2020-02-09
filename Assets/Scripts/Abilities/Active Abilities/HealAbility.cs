using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.Units;
using HeroesOfCode.Battle;

namespace HeroesOfCode.Abilities
{
    public class HealAbility : MonoBehaviour, IActiveAbility
    {
        [SerializeField] private Sprite iconImage = null;
        [SerializeField] private float healPercentage = 0.25f;
        private int amountOfCharges = 1;

        private string description = "";
        private UnitStackBase casterUnit;
        public string GetName() {
            return "Лечение.";
        }
        private void Start() {
            description = "Активная спосбность\r\n\r\n" +
            $"Лечит отряд и воскрешает убитых юнитов. Размер лечения равен {healPercentage * 100}% от здоровья отряда на момент начала сражения.";

            casterUnit = GetComponentInParent<UnitStackBase>();
        }

        public void GetActivated(UnitStackBase unit, BattleManager manager) {
            uint amount = (uint)Mathf.RoundToInt(healPercentage * unit.StartingUnitCount * unit.MaxHealth);
            unit.GetHealedAndResurrected(amount);
            amountOfCharges--;
        }
        public void GetActiatedByAi(UnitStackBase unit, BattleManager manager) {
            uint amount = (uint)Mathf.RoundToInt(healPercentage * unit.StartingUnitCount * unit.MaxHealth);
            unit.GetHealedAndResurrected(amount);
            amountOfCharges--;
        }

        public string GetDescription() {
            return description;
        }

        public Sprite GetImage() {
            return iconImage;
        }


        public bool UnitIsViableTarget(UnitStackBase targetUnit) {
            if (casterUnit.PlayerUnit == targetUnit.PlayerUnit && targetUnit.IsHurt() && casterUnit != targetUnit) {
                return true;
            }
            else {
                return false;
            }
        }

        public bool CanBeUsed() {
            if (amountOfCharges > 0) {
                return true;
            }
            else return false;
        }
    }
}
