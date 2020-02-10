using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.UI;
using HeroesOfCode.Units;

namespace HeroesOfCode.Abilities
{
    public class SkeletonPassive : MonoBehaviour, IPassiveAbility, IDisplayableIcon
    {
        [SerializeField] private Sprite iconImage = null;

        private void Start() {
            StartOfBattleCheck();
        }

        public string GetName() {
            return "Hustle Bones";
        }

        public string GetDescription() {
            return "Пассивная спосбность.\r\n\r\n" +
            "Отряд полностью неузвим к магии.";
        }

        public Sprite GetImage() {
            return iconImage;
        }

        public void StartOfBattleCheck() {
            GetComponentInParent<UnitStackBase>().gameObject
            .AddComponent<MagicInvulnerability>();
        }
    }
}
