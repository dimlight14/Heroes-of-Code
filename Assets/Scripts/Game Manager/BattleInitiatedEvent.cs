using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroesOfCode.Units;

namespace HeroesOfCode
{
    public class BattleInitiatedEvent : CustomEvent
    {
        public Army EnemyArmy;
        public Army PlayerArmy;
    }
}
