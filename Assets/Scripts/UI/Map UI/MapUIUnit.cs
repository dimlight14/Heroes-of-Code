using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HeroesOfCode.UI
{
    public class MapUIUnit : MonoBehaviour
    {
        [SerializeField] private Text unitType = null;
        [SerializeField] private Text numberOfUnits = null;
        [SerializeField] private Text health = null;
        [SerializeField] private Text damage = null;
        [SerializeField] private Text initiative = null;

        public void SetInformation(string name, string numberOfUnits, string maxHealth, string damage, string initiative) {
            unitType.text = name;
            this.numberOfUnits.text = numberOfUnits;
            this.health.text = maxHealth;
            this.damage.text = damage;
            this.initiative.text = initiative;
        }
    }
}
