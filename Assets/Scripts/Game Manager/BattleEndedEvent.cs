using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroesOfCode.Units;

namespace HeroesOfCode
{
    public class BattleEndedEvent : CustomEvent
    {
        public Army playerArmy;
        public Army enemyArmy;
    }
}
