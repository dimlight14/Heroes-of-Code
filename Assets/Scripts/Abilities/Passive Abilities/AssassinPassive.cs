using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.UI;

namespace HeroesOfCode.Abilities
{
    public class AssassinPassive : MonoBehaviour, IPassiveAbility, IDisplayableIcon
    {
        [SerializeField] private Sprite iconImage = null;

        public string GetDescription() {
            return "Пассивная спосбность.\r\n\r\n" +
            "Отряд всегда будет атаковать самый опасный отряд противника.";
        }

        public Sprite GetImage() {
            return iconImage;
        }
        public string GetName() {
            return "Инстинкт убийцы";
        }

        public void StartOfBattleCheck() {
            //does nothing; actual behaviour is in assassin strategy class 
        }
    }
}
