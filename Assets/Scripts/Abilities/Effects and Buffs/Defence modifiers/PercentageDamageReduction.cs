using System.Collections;
using UnityEngine;

using HeroesOfCode.UI;
using HeroesOfCode.Units;

namespace HeroesOfCode.Abilities
{
    public class PercentageDamageReduction : MonoBehaviour, IDisplayIconWrapper, IDefenceModifier
    {
        private Sprite spriteImage;
        private string description = "";
        [SerializeField] private float buffCoefficient;
        private UnitStackBase targetUnit;

        private void Start() {
            description = "Наложенный пасивный эффект.\r\n\r\n" +
            $"Получаемый отрядом урон физический урон уменьшен на {buffCoefficient * 100} %.";
        }
        public string GetName() {
            return "Усиление защиты";
        }
        public void SetEffect(UnitStackBase targetUnit, float buffCoefficient, Sprite spriteImage) {
            this.targetUnit = targetUnit;
            this.buffCoefficient = buffCoefficient;
            this.spriteImage = spriteImage;
        }

        public int ModifyDamageTaken(int initialDamge, UnitStackBase source, DamageType damageType) {
            if (damageType == DamageType.Physical) {
                return Mathf.RoundToInt(initialDamge - (initialDamge * buffCoefficient));
            }
            else {
                return initialDamge;
            }
        }

        public string GetDescription() {
            return description;
        }

        public Sprite GetImage() {
            return spriteImage;
        }
    }
}
