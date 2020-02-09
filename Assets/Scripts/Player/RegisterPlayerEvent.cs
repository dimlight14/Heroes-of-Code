using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroesOfCode.Units;

namespace HeroesOfCode
{
    public class RegisterPlayerEvent : CustomEvent
    {
        public Army PlayerArmy;
        public PlayerTilemapMovement TilemapMovement;
    }
}
