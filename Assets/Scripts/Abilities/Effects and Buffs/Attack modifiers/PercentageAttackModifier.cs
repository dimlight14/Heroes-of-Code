using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.UI;
using HeroesOfCode.Units;

namespace HeroesOfCode.Abilities
{
    public class PercentageAttackModifier : MonoBehaviour, IDisplayIconWrapper, IAttackModifier
    {
        private Sprite spriteImage;
        private string description = "";
        [SerializeField] private float buffCoefficient;
        private UnitStackBase targetUnit;

        private void Start() {
            description = "Наложенный пассивный эффект.\r\n\r\n" +
            $"Весь урон наносимый отрядом увеличен на {Mathf.RoundToInt((buffCoefficient - 1) * 100)}%.";
        }
        public string GetName() {
            return "Усиление урона.";
        }

        public void SetEffect(UnitStackBase targetUnit, float buffCoefficient, Sprite spriteImage) {
            this.targetUnit = targetUnit;
            this.buffCoefficient = buffCoefficient;
            this.spriteImage = spriteImage;
        }

        public int ModifyDamage(int initialDamge) {
            return Mathf.RoundToInt(initialDamge * buffCoefficient);
        }

        public string GetDescription() {
            return description;
        }

        public Sprite GetImage() {
            return spriteImage;
        }
    }
}
