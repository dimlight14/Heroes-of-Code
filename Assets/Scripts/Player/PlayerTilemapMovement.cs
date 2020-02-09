using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using HeroesOfCode.Tilemap;

namespace HeroesOfCode
{
    public class PlayerTilemapMovement : MonoBehaviour
    {
        [SerializeField]
        private float movementStepDelay = 0.5f;

        private Queue<Vector2> path;
        private bool isMoving = false;

        public void SetPath(List<Vector2> path) {
            if (path.Count == 0) return;

            if (isMoving) {
                StopMovement();
            }
            else {
                this.path = new Queue<Vector2>(path);
                StartMovement();
            }
        }

        private void StartMovement() {
            if (isMoving) StopMovement();

            isMoving = true;
            StartCoroutine("MovePlayer");
        }
        private IEnumerator MovePlayer() {
            while (path.Count != 0) {
                MoveTo(path.Dequeue());
                yield return new WaitForSeconds(movementStepDelay);
            }
            EndMovement();
        }
        public void StopMovement() {
            if (!isMoving) return;

            StopCoroutine("MovePlayer");
            isMoving = false;
        }
        private void EndMovement() {
            StopMovement();
            EventBus.FireEvent<PlayerMovementEnded>();
        }

        private void MoveTo(Vector2 newPosition) {
            transform.position = newPosition;
            EventBus.FireEvent<PlayerMovedEvent>();
        }
    }
}
