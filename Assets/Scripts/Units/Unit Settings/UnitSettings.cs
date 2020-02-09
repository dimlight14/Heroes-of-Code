using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesOfCode.Units
{
    [CreateAssetMenu(menuName = "Unit Settings")]
    public class UnitSettings : ScriptableObject
    {
        public UnitData baseData;
        public GameObject basePrefab;
    }
}
