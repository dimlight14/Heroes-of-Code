using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesOfCode.Units
{
    [CreateAssetMenu(menuName = "Unit Settings Set")]
    public class UnitSettingsSet : ScriptableObject
    {
        [System.Serializable]
        private struct ValuePair
        {
            public UnitType key;
            public UnitSettings value;
        }

        [SerializeField]
        private List<ValuePair> unitSettingsList = null;

        [HideInInspector]
        public Dictionary<UnitType, UnitSettings> unitSettingsMap {
            get {
                var dictionary = new Dictionary<UnitType, UnitSettings>();
                foreach (var keyPair in unitSettingsList) {
                    dictionary.Add(keyPair.key, keyPair.value);
                }
                return dictionary;
            }
        }
    }
}
