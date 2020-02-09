using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using HeroesOfCode.Battle;
using HeroesOfCode.Units;

namespace HeroesOfCode.Abilities
{
    public class DefendAbility : MonoBehaviour, IActiveAbility, IUpdateEffectOnDeath
    {
        [SerializeField] private float selfBuffCoefficient = 0.25f;
        [SerializeField] private Sprite iconImage = null;

        private int amountOfCharges = 1;
        private string description = "";
        private UnitStackBase casterUnit;
        private RecievedAttackRedirection defenderEffect;

        private void Start() {
            description = "Активная спосбность\r\n\r\n" +
            $"Защищает союзную цель от любых физических атак. Дополнительно, отряд использующий способность получает {selfBuffCoefficient * 100}% сопротивления физическому урону.";

            casterUnit = GetComponentInParent<UnitStackBase>();
        }
        public string GetName() {
            return "Защита";
        }

        public bool CanBeUsed() {
            if (amountOfCharges > 0) {
                return true;
            }
            else return false;
        }

        public void GetActiatedByAi(UnitStackBase targetUnit, BattleManager manager) {
            DefendTarget(targetUnit);
            amountOfCharges--;
        }

        public void GetActivated(UnitStackBase targetUnit, BattleManager manager) {
            DefendTarget(targetUnit);
            amountOfCharges--;
        }

        private void DefendTarget(UnitStackBase targetUnit) {
            PercentageDamageReduction modifier = casterUnit.gameObject.AddComponent<PercentageDamageReduction>();
            modifier.SetEffect(casterUnit, selfBuffCoefficient, iconImage);

            defenderEffect = targetUnit.gameObject.AddComponent<RecievedAttackRedirection>();
            defenderEffect.SetEffect(targetUnit, casterUnit, iconImage);

            Debug.Log($"{targetUnit} is now defended by {casterUnit}.");
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

        public void UpdateOnDeath() {
            Destroy(defenderEffect);
        }
    }
}
