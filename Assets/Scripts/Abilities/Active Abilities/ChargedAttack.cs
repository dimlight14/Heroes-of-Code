using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.Battle;
using HeroesOfCode.Units;

namespace HeroesOfCode.Abilities
{
    public class ChargedAttack : MonoBehaviour, IActiveAbility
    {
        [SerializeField] private Sprite iconImage = null;
        [SerializeField] private uint flatDamage = 20;
        [SerializeField] private float allDamagePercentage = 0.8f;
        private int amountOfCharges = 1;

        private UnitStackBase casterUnit;

        private void Start() {
            casterUnit = GetComponentInParent<UnitStackBase>();
        }
        public string GetName() {
            return "Заряженная Атака";
        }

        public void GetActivated(UnitStackBase targetUnit, BattleManager manager) {
            MakeAChargedAttack(targetUnit);
            amountOfCharges--;
        }
        public void GetActiatedByAi(UnitStackBase targetUnit, BattleManager manager) {
            MakeAChargedAttack(targetUnit);
            amountOfCharges--;
        }
        private void MakeAChargedAttack(UnitStackBase targetUnit) {
            targetUnit.GetDamaged(CalculateDamage(), casterUnit, DamageType.Magical);
        }
        private int CalculateDamage() {
            return Mathf.RoundToInt(flatDamage + allDamagePercentage * casterUnit.DamageDealtThisBattle);
        }

        public string GetDescription() {
            return "Активная спосбность\r\n\r\n" +
            $"Наносит урон вражескому отряду. Чем больше урона нанесено отрядом в бою, тем выше урон способности. Текущий урон: {CalculateDamage()}.";
        }

        public Sprite GetImage() {
            return iconImage;
        }


        public bool UnitIsViableTarget(UnitStackBase targetUnit) {
            if (casterUnit.PlayerUnit != targetUnit.PlayerUnit) {
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
