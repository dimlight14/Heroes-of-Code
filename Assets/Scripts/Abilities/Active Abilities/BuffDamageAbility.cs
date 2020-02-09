using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.Units;
using HeroesOfCode.Battle;

namespace HeroesOfCode.Abilities
{
    public class BuffDamageAbility : MonoBehaviour, IActiveAbility
    {
        [SerializeField] private float buffCoefficientPerUnit = 0.1f;
        [SerializeField] private Sprite iconImage = null;

        private int amountOfCharges = 1;
        private string description = "";
        private UnitStackBase casterUnit;

        private void Start() {
            casterUnit = GetComponentInParent<UnitStackBase>();
            description = "Активная спосбность\r\n\r\n" +
            $"Увеличивает ЛЮБОЙ урон наносимый союзным отрядом на {casterUnit.NumberOfUnits} * {buffCoefficientPerUnit * 100}%.";
        }
        public string GetName() {
            return "Заклятье силы.";
        }

        public bool CanBeUsed() {
            if (amountOfCharges > 0) {
                return true;
            }
            else return false;
        }

        public void GetActiatedByAi(UnitStackBase unit, BattleManager manager) {
            AttachBuffTo(unit);
            amountOfCharges--;
        }

        public void GetActivated(UnitStackBase unit, BattleManager manager) {
            AttachBuffTo(unit);
            amountOfCharges--;
        }

        private void AttachBuffTo(UnitStackBase targetUnit) {
            PercentageAttackModifier buffObject = targetUnit.gameObject.AddComponent<PercentageAttackModifier>();
            buffObject.SetEffect(targetUnit, CalculateBuff(), iconImage);
        }
        private float CalculateBuff() {
            return 1 + buffCoefficientPerUnit * casterUnit.NumberOfUnits;
        }

        public string GetDescription() {
            return description;
        }

        public Sprite GetImage() {
            return iconImage;
        }

        public bool UnitIsViableTarget(UnitStackBase targetUnit) {
            if (casterUnit.PlayerUnit == targetUnit.PlayerUnit && casterUnit != targetUnit) {
                return true;
            }
            else {
                return false;
            }
        }
    }
}
