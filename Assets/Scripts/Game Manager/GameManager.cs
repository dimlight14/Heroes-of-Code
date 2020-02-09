using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesOfCode
{
    public class GameManager : MonoBehaviour
    {
        private Camera mainCamera;

        private void Awake() {
            EventBus.Subscribe<PreBattleInitiatedEvent>(OnBattleInitiated);
            EventBus.Subscribe<BattleEndedEvent>(OnBattleEnded);
            mainCamera = Camera.main;
        }

        private void OnBattleEnded(BattleEndedEvent customEvent) {
            mainCamera.transform.position = new Vector3(0.5f, -1, -10);
        }
        private void OnBattleInitiated(PreBattleInitiatedEvent customEvent) {
            mainCamera.transform.position = new Vector3(40, 0, -10);
        }
    }
}
