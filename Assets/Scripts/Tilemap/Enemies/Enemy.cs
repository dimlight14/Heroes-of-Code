using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroesOfCode.Units;

namespace HeroesOfCode.Tilemap
{
    [SelectionBase]
    public class Enemy : MonoBehaviour
    {
        public Vector2Int currentGridPosition;
        [HideInInspector] public Army army;

        private void Awake() {
            army = GetComponent<Army>();
        }

        private void Start() {
            EventBus.FireEvent<EnemyCreatedEvent>(new EnemyCreatedEvent() { Enemy = this });
        }
    }
}
