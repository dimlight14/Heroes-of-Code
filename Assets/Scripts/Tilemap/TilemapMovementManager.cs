using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.Units;

namespace HeroesOfCode.Tilemap
{
    public class TilemapMovementManager : MonoBehaviour
    {
        [SerializeField] private TilemapManager tilemap = null;


        private PlayerTilemapMovement playerMovement;
        private Army playerArmy;
        private AStarPathFinding pathFinding;
        private bool movementIsBlocked = false;
        private Vector2 selectedPosition;
        private List<Vector2Int> currPath;
        private Camera mainCamera;
        private bool needStarPathReset = false;

        private void Awake() {
            pathFinding = new AStarPathFinding(tilemap);
            mainCamera = Camera.main;
            EventBus.Subscribe<BattleEndedEvent>(OnBattleEnded);
            EventBus.Subscribe<PlayerMovementEnded>(PlayerMovementEndedCallback);
            EventBus.Subscribe<PlayerMovedEvent>(PlayerMovementStep);
            EventBus.Subscribe<TileCreatedEvent>(OnTileCreated);
            EventBus.Subscribe<EnemyCreatedEvent>(OnEnemyCreated);
            EventBus.Subscribe<RegisterPlayerEvent>(OnPlayerRegister);
        }

        private void OnPlayerRegister(RegisterPlayerEvent customEvent) {
            playerMovement = customEvent.TilemapMovement;
            playerArmy = customEvent.PlayerArmy;
        }

        private void OnEnemyCreated(EnemyCreatedEvent customEvent) {
            needStarPathReset = true;
        }

        private void OnTileCreated(TileCreatedEvent customEvent) {
            needStarPathReset = true;
        }

        private void OnBattleEnded(BattleEndedEvent customEvent) {
            movementIsBlocked = false;
            needStarPathReset = true;
        }

        private void Update() {
            if (!movementIsBlocked && Input.GetMouseButtonDown(0)) {
                Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                OnMouseClicked(mousePos);
            }
        }
        private void OnMouseClicked(Vector2 mousePos) {
            tilemap.SetEnemiesInfluence();
            Vector2Int adjustedMousePos = tilemap.FromWorldToGrid(mousePos);
            if (adjustedMousePos.x >= tilemap.tiles.GetLength(0) || adjustedMousePos.x < 0) {
                return;
            }
            if (adjustedMousePos.y >= tilemap.tiles.GetLength(1) || adjustedMousePos.y < 0) {
                return;
            }

            if (selectedPosition == adjustedMousePos) {
                MovePlayer();
                movementIsBlocked = true;
                return;
            }

            if (adjustedMousePos != tilemap.FromWorldToGrid(playerMovement.transform.position)) {
                if (!tilemap.tiles[adjustedMousePos.x, adjustedMousePos.y].CanBePathEnd()) return;

                selectedPosition = adjustedMousePos;
                CreatePath(adjustedMousePos);
            }
        }
        private void MovePlayer() {
            if (currPath == null) return;

            List<Vector2> vectorPath = new List<Vector2>();
            for (int i = 0; i < currPath.Count; i++) {
                Vector2 node = currPath[i];
                vectorPath.Add(new Vector2(node.x + tilemap.bottomLeftCorner.x, node.y + tilemap.bottomLeftCorner.y));
            }
            playerMovement.SetPath(vectorPath);
        }
        private void CreatePath(Vector2Int endCell) {
            if (needStarPathReset) pathFinding.ResetTilemap(tilemap.tiles);

            ClearPreviousPath();
            currPath = pathFinding.FindPath(tilemap.FromWorldToGrid(playerMovement.transform.position), endCell);
            DrawPath();
        }
        private void DrawPath() {
            if (currPath == null) return;
            for (int i = 0; i < currPath.Count - 1; i++) {
                tilemap.tiles[currPath[i].x, currPath[i].y].SetPathImage(
                    new Vector2(currPath[i].x, currPath[i].y),
                    new Vector2(currPath[i + 1].x, currPath[i + 1].y)
                );
            }
            tilemap.tiles[currPath[currPath.Count - 1].x, currPath[currPath.Count - 1].y].SetPathImageAsEnd();
        }
        private void ClearPreviousPath() {
            if (currPath == null) return;
            foreach (Vector2Int node in currPath) {
                tilemap.tiles[node.x, node.y].ClearPathImage();
            }
        }

        private void PlayerMovementStep(PlayerMovedEvent customEvent) {
            tilemap.tiles[currPath[0].x, currPath[0].y].ClearPathImage();
            currPath.RemoveAt(0);
        }
        private void PlayerMovementEndedCallback(PlayerMovementEnded customEvent) {
            ClearPreviousPath();

            Vector2Int playerPosition = tilemap.FromWorldToGrid(playerMovement.transform.position);
            if (tilemap.tiles[playerPosition.x, playerPosition.y].adjacentToEnemy == true) {
                List<Tile> neighbours = tilemap.GetNeighbours(tilemap.tiles[playerPosition.x, playerPosition.y]);
                Tile enemyTile = null;
                foreach (Tile neighbour in neighbours) {
                    if (neighbour.containsEnemy) {
                        enemyTile = neighbour;
                        break;
                    }
                }
                foreach (Enemy enemy in tilemap.enemies) {
                    if (enemy.currentGridPosition == enemyTile.gridPosition) {
                        EventBus.FireEvent<PreBattleInitiatedEvent>();
                        EventBus.FireEvent<BattleInitiatedEvent>(new BattleInitiatedEvent() { EnemyArmy = enemy.army, PlayerArmy = playerArmy });
                        break;
                    }
                }
            }
            else {
                movementIsBlocked = false;
            }
        }
    }
}
