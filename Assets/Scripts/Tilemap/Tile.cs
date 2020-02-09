using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesOfCode.Tilemap
{
    public class Tile : MonoBehaviour
    {
        public enum Type
        {
            Grass,
            Rocks
        }


        [SerializeField]
        private Tile.Type tileType = Tile.Type.Grass;
        [SerializeField]
        private TilePathImageController pathImageController = null;
        public Tile.Type TileType { get => tileType; }
        public bool containsEnemy = false;
        public bool adjacentToEnemy = false;
        public Vector2Int gridPosition;


        private void Start() {
            EventBus.FireEvent<TileCreatedEvent>(new TileCreatedEvent() { Tile = this });
        }

        public void ClearPathImage() {
            pathImageController.ClearPathImage();
        }
        public void SetPathImageAsEnd() {
            pathImageController.SetPathImageAsEnd();
        }
        public void SetPathImage(Vector2 prevPos, Vector2 thisPos) {
            pathImageController.SetPathImage(prevPos, thisPos);
        }

        public bool IsWalkable() {
            if (containsEnemy || adjacentToEnemy) return false;

            switch (tileType) {
                case Tile.Type.Grass:
                    return true;
                case Tile.Type.Rocks:
                    return false;
                default:
                    return false;
            }
        }
        public bool CanBePathEnd() {
            if (containsEnemy) return false;
            switch (tileType) {
                case Tile.Type.Grass:
                    return true;
                case Tile.Type.Rocks:
                    return false;
                default:
                    return false;
            }
        }
    }
}
