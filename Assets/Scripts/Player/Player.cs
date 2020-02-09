using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroesOfCode.Units;

namespace HeroesOfCode
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private Army playerArmy = null;
        [SerializeField] private PlayerTilemapMovement tilemapMovement = null;

        private void Start() {
            EventBus.FireEvent<RegisterPlayerEvent>(new RegisterPlayerEvent() { PlayerArmy = playerArmy, TilemapMovement = tilemapMovement });
        }
    }
}
