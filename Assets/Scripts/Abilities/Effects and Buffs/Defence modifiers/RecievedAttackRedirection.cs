using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.UI;
using HeroesOfCode.Units;

namespace HeroesOfCode.Abilities
{
    public class RecievedAttackRedirection : MonoBehaviour, IDisplayableIcon, IDefenceModifier
    {
        [SerializeField] private UnitStackBase protectorUnit;
        private Sprite spriteImage;
        private string description = "";
        private UnitStackBase targetUnit;

        private void Start() {
            description = "Наложенный эффект.\r\n\r\n" +
            $"Отряд защищен от физических атак. Весь физический урон будет перенаправлен на защищающий отряд.";
        }
        public string GetName() {
            return "Защита";
        }

        public void SetEffect(UnitStackBase targetUnit, UnitStackBase protectorUnit, Sprite spriteImage) {
            this.targetUnit = targetUnit;
            this.protectorUnit = protectorUnit;
            this.spriteImage = spriteImage;
        }

        public int ModifyDamageTaken(int initialDamge, UnitStackBase source, DamageType damageType) {
            if (damageType == DamageType.Physical) {
                RedirectAttack(initialDamge, source, damageType);
                return 0;
            }
            else {
                return initialDamge;
            }
        }

        private void RedirectAttack(int damage, UnitStackBase source, DamageType damageType) {
            if (!protectorUnit.gameObject.activeInHierarchy) {
                return;
            }

            protectorUnit.GetDamaged(damage, source, damageType);
        }



        public string GetDescription() {
            return description;
        }

        public Sprite GetImage() {
            return spriteImage;
        }
    }
}
