using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesOfCode.Abilities
{
    [CreateAssetMenu(menuName = "Abilities' Settings")]
    public class AbilitiesSettings : ScriptableObject
    {
        [System.Serializable]
        private struct ValuePair
        {
            public AbilityType key;
            public GameObject value;
        }

        [SerializeField]
        private List<ValuePair> abilitiesPrefabs = null;

        [HideInInspector]
        public Dictionary<AbilityType, GameObject> abilitiesPrefabMap {
            get {
                var dictionary = new Dictionary<AbilityType, GameObject>();
                foreach (var keyPair in abilitiesPrefabs) {
                    dictionary.Add(keyPair.key, keyPair.value);
                }
                return dictionary;
            }
        }
    }
}
