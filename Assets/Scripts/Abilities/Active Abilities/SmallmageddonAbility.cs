using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.Battle;
using HeroesOfCode.Units;

namespace HeroesOfCode.Abilities
{
    public class SmallmageddonAbility : MonoBehaviour, IActiveAbility
    {
        [SerializeField] private Sprite iconImage = null;
        [SerializeField] private int damageAmount = 15;
        private int amountOfCharges = 1;

        private string description = "";
        private UnitStackBase casterUnit;

        private void Start() {
            description = "Активная спосбность\r\n\r\n" +
            $"Как армагеддон, только меньше. Наносит {damageAmount} магического урона всем отрядам на поле боя.";

            casterUnit = GetComponentInParent<UnitStackBase>();
        }
        public string GetName() {
            return "Малмагеддон";
        }
        public void GetActivated(UnitStackBase unit, BattleManager manager) {
            DamgeAllUnits(manager);
            amountOfCharges--;
        }
        public void GetActiatedByAi(UnitStackBase unit, BattleManager manager) {
            DamgeAllUnits(manager);
            amountOfCharges--;
        }
        private void DamgeAllUnits(BattleManager manager) {
            List<UnitStackBase> allUnits = new List<UnitStackBase>();
            allUnits.AddRange(manager.playerUnitsInBattle);
            allUnits.AddRange(manager.enemyUnitsInBattle);
            foreach (var unit in allUnits) {
                unit.GetDamaged(CalculateDamage(), casterUnit, DamageType.Magical);
            }
        }
        private int CalculateDamage() {
            List<PercentageAttackModifier> damageModifiers = new List<PercentageAttackModifier>();
            casterUnit.GetComponents<PercentageAttackModifier>(damageModifiers);
            if (damageModifiers.Count == 0) {
                return damageAmount;
            }
            else {
                int finalDamage = damageAmount;
                foreach (PercentageAttackModifier modifier in damageModifiers) {
                    finalDamage = modifier.ModifyDamage(finalDamage);
                }
                return finalDamage;
            }
        }

        public string GetDescription() {
            return description;
        }

        public Sprite GetImage() {
            return iconImage;
        }


        public bool UnitIsViableTarget(UnitStackBase targetUnit) {
            return true;
        }

        public bool CanBeUsed() {
            if (amountOfCharges > 0) {
                return true;
            }
            else return false;
        }
    }
}
