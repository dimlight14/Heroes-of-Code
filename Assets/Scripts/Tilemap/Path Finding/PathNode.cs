using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesOfCode.Tilemap
{
    public class PathNode
    {
        public int x, y;
        public int gCost, hCost;
        public int FCost { get => gCost + hCost; }
        public bool isWalkable;
        public PathNode cameFromNode = null;


        public PathNode(Vector2Int gridPostition, bool isWalkable) {
            this.isWalkable = isWalkable;
            x = gridPostition.x;
            y = gridPostition.y;
        }
    }
}
