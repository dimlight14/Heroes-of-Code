using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.Abilities;
namespace HeroesOfCode.Units
{
    [System.Serializable]
    public class UnitData
    {
        public UnitType type;
        public string unitName;
        public uint maxHealth;
        public uint damage;
        public int initiative;

        public AbilityType ability;
    }
}
