using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.Abilities;

namespace HeroesOfCode.Units
{
    public class UnitStackFactory : MonoBehaviour
    {
        [SerializeField] private UnitSettingsSet unitSettingsSet = null;
        [SerializeField] private AbilitiesSettings abilitiesSettings = null;

        private Dictionary<AbilityType, GameObject> abilitiesMap;
        private Dictionary<UnitType, UnitSettings> unitSettingsMap;

        private void Awake() {
            CacheSettings();
        }

        private void CacheSettings() {
            abilitiesMap = abilitiesSettings.abilitiesPrefabMap;
            unitSettingsMap = unitSettingsSet.unitSettingsMap;
        }

        public UnitStackBase Create(UnitStackRepresentation unit, bool playerUnit) {
            UnitSettings settings = unitSettingsMap[unit.type];
            GameObject newUnit = Instantiate(settings.basePrefab, transform.position, Quaternion.identity, transform);
            UnitStackBase unitBaseClass = newUnit.GetComponent<UnitStackBase>();
            if (unitBaseClass == null) {
                Debug.LogError("Prefab doesn't contain an instance of UnitStackBase class.");
                return null;
            }
            unitBaseClass.SetBaseSettings(unit.type, unit.numberOfUnits,
                playerUnit, settings.baseData.maxHealth,
                settings.baseData.damage, settings.baseData.initiative,
                settings.baseData.unitName
             );

            if (settings.baseData.ability != AbilityType.None) {
                AddAbility(unitBaseClass, settings.baseData.ability);
            }

            return unitBaseClass;
        }

        private void AddAbility(UnitStackBase unit, AbilityType ability) {
            if (!abilitiesMap.ContainsKey(ability)) {
                Debug.LogError($"The factory doesn't have settings for the ability of type {ability}.");
                return;
            }

            Instantiate(abilitiesMap[ability], new Vector3(), Quaternion.identity, unit.transform);
        }
    }
}
