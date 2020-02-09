using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using HeroesOfCode.Abilities;

namespace HeroesOfCode.Units
{
    [System.Serializable]
    public struct UnitStackRepresentation
    {
        public UnitType type;
        public uint numberOfUnits;
    }
    [SelectionBase]
    public class UnitStackBase : MonoBehaviour
    {
        public UnitSelection unitSelection;
        public UnitType UnitType { get => unitType; }
        public uint NumberOfUnits { get => numberOfUnits; private set => numberOfUnits = value; }
        public uint MaxHealth { get => maxHealth; private set => maxHealth = value; }
        public uint TopUnitHealth { get => topUnitHealth; private set => topUnitHealth = value; }
        public int Initiative { get => initiative; private set => initiative = value; }
        public bool PlayerUnit { get => playerUnit; private set => playerUnit = value; }
        public string UnitName { get => unitName; private set => unitName = value; }
        public int DamageDealtThisBattle { get => damageDealtThisBattle; private set => damageDealtThisBattle = value; }
        public uint StartingUnitCount { get => startingUnitCount; private set => startingUnitCount = value; }
        public uint Damage { get => damage; set => damage = value; }

        private UnitType unitType;
        private uint numberOfUnits;
        private uint maxHealth;
        private uint topUnitHealth;
        private int initiative;
        private bool playerUnit;
        private string unitName;
        private uint damage;
        private Text numberOfUnitsPlaque;
        private int damageDealtThisBattle;
        private uint startingUnitCount;

        public void SetBaseSettings(UnitType unitType, uint numberOfUnits, bool playerUnit, uint maxHealth, uint damage, int initiative, string unitName) {
            this.unitType = unitType;
            this.NumberOfUnits = this.StartingUnitCount = numberOfUnits;
            this.PlayerUnit = playerUnit;
            this.MaxHealth = this.TopUnitHealth = maxHealth;
            this.Damage = damage;
            this.Initiative = initiative;
            this.UnitName = unitName;

            if (!playerUnit) MirrorImageAndSelection();
        }

        private void MirrorImageAndSelection() {
            GetComponentInChildren<SpriteRenderer>().flipX = true;
            GameObject selectionObject = GetComponentInChildren<UnitSelection>().gameObject;
            selectionObject.transform.localScale = new Vector3(-1, 1, 1);
        }

        public void SetNumberOfUnitsPlaque(Text plaque) {
            numberOfUnitsPlaque = plaque;
            plaque.text = NumberOfUnits.ToString();
        }

        public int GetAttackDamage() {
            int damage = Mathf.RoundToInt(NumberOfUnits * this.Damage);
            List<IAttackModifier> attackModifiers = new List<IAttackModifier>();
            GetComponents<IAttackModifier>(attackModifiers);

            if (attackModifiers.Count != 0) {
                foreach (var modifier in attackModifiers) {
                    damage = modifier.ModifyDamage(damage);
                }
            }

            return damage;
        }

        public bool IsTargetableByAttack() {
            return true;
        }
        public bool IsHurt() {
            if (NumberOfUnits < StartingUnitCount || TopUnitHealth < MaxHealth) {
                return true;
            }
            else {
                return false;
            }
        }

        public void GetDamaged(int damage, UnitStackBase source, DamageType damageType) {
            int damageRecieved = damage;
            List<IDefenceModifier> defenceModifiers = new List<IDefenceModifier>();
            GetComponents<IDefenceModifier>(defenceModifiers);

            if (defenceModifiers.Count != 0) {
                foreach (var modifier in defenceModifiers) {
                    damageRecieved = modifier.ModifyDamageTaken(damageRecieved, source, damageType);
                }
            }

            if (source != null) source.DamageDealtThisBattle += damageRecieved;
            ApplyDamage(damageRecieved);
            Debug.Log($"{source.UnitType} attacked {UnitType} for {damage} damage.");
        }
        private void ApplyDamage(int damage) {
            int remainder = (int)(damage % MaxHealth);
            if (remainder >= TopUnitHealth) {
                NumberOfUnits--;
                if (NumberOfUnits == 0) {
                    Die();
                    return;
                }
                TopUnitHealth = (uint)Mathf.Max(MaxHealth - (TopUnitHealth - remainder), 1);
            }
            else {
                TopUnitHealth = (uint)Mathf.Max(TopUnitHealth - remainder, 1);
            }

            int noRemainderDamage = damage - remainder;
            noRemainderDamage = Mathf.RoundToInt(noRemainderDamage / MaxHealth);
            if (noRemainderDamage >= NumberOfUnits) {
                Die();
                return;
            }
            else {
                NumberOfUnits = (uint)(NumberOfUnits - noRemainderDamage);
            }

            numberOfUnitsPlaque.text = NumberOfUnits.ToString();
        }

        public void GetHealedAndResurrected(uint healAmount) {
            TopUnitHealth += healAmount;
            uint remainder = TopUnitHealth % MaxHealth;
            uint unitsResurrected = (uint)Mathf.Max(TopUnitHealth / MaxHealth);

            uint newUnitsCount = NumberOfUnits + unitsResurrected;
            if (newUnitsCount > StartingUnitCount) {
                NumberOfUnits = StartingUnitCount;
                TopUnitHealth = MaxHealth;
            }
            else {
                NumberOfUnits = newUnitsCount;
                TopUnitHealth = remainder;
            }
            numberOfUnitsPlaque.text = NumberOfUnits.ToString();
        }
        private void Die() {
            Destroy(numberOfUnitsPlaque.transform.parent.gameObject);
            List<IUpdateEffectOnDeath> effects = new List<IUpdateEffectOnDeath>();
            effects.AddRange(GetComponentsInChildren<IUpdateEffectOnDeath>());
            foreach (var effect in effects) {
                effect.UpdateOnDeath();
            }
            EventBus.FireEvent<UnitDiedEvent>(new UnitDiedEvent() { Unit = this });
        }

        public void DestroySelf() {
            if (numberOfUnitsPlaque != null) Destroy(numberOfUnitsPlaque.transform.parent.gameObject);

            Destroy(gameObject);
        }
    }
}