using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HeroesOfCode.Units;

namespace HeroesOfCode.Tilemap
{
    public class TilemapManager : MonoBehaviour
    {
        [SerializeField]
        private uint width = 0, height = 0;
        [SerializeField] public Vector2 bottomLeftCorner;

        [HideInInspector] public Tile[,] tiles;
        [HideInInspector] public List<Enemy> enemies = new List<Enemy>();
        private bool enemiesInfluenceSet = false;

        private void Awake() {
            tiles = new Tile[width, height];
            EventBus.Subscribe<TileCreatedEvent>(OnTileCreated);
            EventBus.Subscribe<EnemyCreatedEvent>(OnEnemyCreated);
            EventBus.Subscribe<BattleEndedEvent>(OnBattleEnded);
        }

        private void OnBattleEnded(BattleEndedEvent customEvent) {
            RemoveEnemy(customEvent.enemyArmy);
        }

        #region Tiles
        private void OnTileCreated(TileCreatedEvent customEvent) {
            Vector2Int newTilePosition = FromWorldToGrid(customEvent.Tile.transform.position);
            if (tiles[newTilePosition.x, newTilePosition.y] == null) {
                tiles[newTilePosition.x, newTilePosition.y] = customEvent.Tile;
                customEvent.Tile.gridPosition = FromWorldToGrid(customEvent.Tile.transform.position);
            }
            else {
                Debug.LogError($"Tilemap has already registered a tile at the position {customEvent.Tile.transform.position}.");
                Destroy(customEvent.Tile);
            }
        }
        public List<Tile> GetNeighbours(Tile tile) {
            List<Tile> neighbours = new List<Tile>();
            Vector2Int tilePosition = FromWorldToGrid(tile.transform.position);
            if (tilePosition.x - 1 >= 0) {
                //Left
                if (tiles[tilePosition.x - 1, tilePosition.y] != null) neighbours.Add(tiles[tilePosition.x - 1, tilePosition.y]);
                //Left Down
                if (tilePosition.y - 1 >= 0 && tiles[tilePosition.x - 1, tilePosition.y - 1] != null) neighbours.Add(tiles[tilePosition.x - 1, tilePosition.y - 1]);
                //Left Up
                if (tilePosition.y + 1 < tiles.GetLength(1) && tiles[tilePosition.x - 1, tilePosition.y + 1] != null) neighbours.Add(tiles[tilePosition.x - 1, tilePosition.y + 1]);
            }
            if (tilePosition.x + 1 < tiles.GetLength(0)) {
                //Right
                if (tiles[tilePosition.x + 1, tilePosition.y] != null) neighbours.Add(tiles[tilePosition.x + 1, tilePosition.y]);
                //Right Down
                if (tilePosition.y - 1 >= 0 && tiles[tilePosition.x + 1, tilePosition.y - 1] != null) neighbours.Add(tiles[tilePosition.x + 1, tilePosition.y - 1]);
                //Right Up
                if (tilePosition.y + 1 < tiles.GetLength(1) && tiles[tilePosition.x + 1, tilePosition.y + 1] != null) neighbours.Add(tiles[tilePosition.x + 1, tilePosition.y + 1]);
            }
            //Down
            if (tilePosition.y - 1 >= 0 && tiles[tilePosition.x, tilePosition.y - 1] != null)
                neighbours.Add(tiles[tilePosition.x, tilePosition.y - 1]);
            //Up
            if (tilePosition.y + 1 < tiles.GetLength(1) && tiles[tilePosition.x, tilePosition.y + 1] != null)
                neighbours.Add(tiles[tilePosition.x, tilePosition.y + 1]);

            return neighbours;
        }
        #endregion

        #region Enemies
        private void RemoveEnemy(Army enemyArmy) {
            Enemy enemyToRemove = null;
            foreach (Enemy enemy in enemies) {
                if (enemy.army == enemyArmy) {
                    enemyToRemove = enemy;
                    break;
                }
            }

            if (enemyToRemove != null) {
                List<Tile> neighbours = GetNeighbours(tiles[enemyToRemove.currentGridPosition.x, enemyToRemove.currentGridPosition.y]);
                tiles[enemyToRemove.currentGridPosition.x, enemyToRemove.currentGridPosition.y].containsEnemy = false;
                foreach (Tile neighbour in neighbours) {
                    neighbour.adjacentToEnemy = false;
                }
                enemies.Remove(enemyToRemove);
                Destroy(enemyToRemove.gameObject);
            }
        }
        private void OnEnemyCreated(EnemyCreatedEvent customEvent) {
            if (!enemies.Contains(customEvent.Enemy)) enemies.Add(customEvent.Enemy);
        }
        public void SetEnemiesInfluence() {
            if (enemiesInfluenceSet) return;

            Vector2Int enemyPosition;
            List<Tile> neighbours;
            foreach (Enemy enemy in enemies) {
                enemyPosition = FromWorldToGrid(enemy.transform.position);
                enemy.currentGridPosition = enemyPosition;//error prone,must be aligned?
                if (tiles[enemyPosition.x, enemyPosition.y] == null) {
                    //error
                    continue;
                }
                tiles[enemyPosition.x, enemyPosition.y].containsEnemy = true;
                neighbours = GetNeighbours(tiles[enemyPosition.x, enemyPosition.y]);
                foreach (Tile neighbour in neighbours) {
                    neighbour.adjacentToEnemy = true;
                }
            }
            enemiesInfluenceSet = true;
        }
        #endregion

        public Vector2Int FromWorldToGrid(Vector2 worldPos) {
            Vector2Int newPos = new Vector2Int();
            newPos.x = Mathf.RoundToInt(worldPos.x - bottomLeftCorner.x);
            newPos.y = Mathf.RoundToInt(worldPos.y - bottomLeftCorner.y);
            return newPos;
        }
    }
}
