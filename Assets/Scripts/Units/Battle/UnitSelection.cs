using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using HeroesOfCode.UI;

namespace HeroesOfCode.Units
{
    public class UnitSelection : MonoBehaviour, IPointerClickHandler
    {
        public enum SelectionType
        {
            Active,
            MouseOver,
            TargetSelected,
            None
        }
        [SerializeField] private SpriteRenderer spriteRenderer = null;
        [Space(20)]
        [SerializeField] private Sprite activeSelectionSprite = null;
        [SerializeField] private Sprite targetSelectionSprite = null;
        [SerializeField] private Sprite mouseOverSelectionSprite = null;

        private UnitStackBase unitStackBase;

        private SelectionType selectionStatus = SelectionType.None;

        private void Start() {
            unitStackBase = GetComponentInParent<UnitStackBase>();
        }

        public void SetSelectionImage(SelectionType selectionType) {
            selectionStatus = selectionType;
            switch (selectionType) {
                case SelectionType.Active:
                    spriteRenderer.sprite = activeSelectionSprite;
                    break;
                case SelectionType.MouseOver:
                    spriteRenderer.sprite = mouseOverSelectionSprite;
                    break;
                case SelectionType.TargetSelected:
                    spriteRenderer.sprite = targetSelectionSprite;
                    break;
                case SelectionType.None:
                    spriteRenderer.sprite = null;
                    break;
            }
        }

        private void OnMouseEnter() {
            if (selectionStatus == SelectionType.None) {
                SetSelectionImage(SelectionType.MouseOver);
            }
        }
        private void OnMouseExit() {
            if (selectionStatus == SelectionType.MouseOver) {
                SetSelectionImage(SelectionType.None);
            }
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Left) {
                if (selectionStatus != SelectionType.Active && selectionStatus != SelectionType.TargetSelected) {
                    EventBus.FireEvent<UnitClickedEvent>(new UnitClickedEvent() { Unit = unitStackBase, UnitSelection = this });
                    SetSelectionImage(SelectionType.TargetSelected);
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Right) {
                EventBus.FireEvent<Battle.PlayerInteractedWithUnitEvent>(new Battle.PlayerInteractedWithUnitEvent() { TargetUnit = unitStackBase });
            }
        }
    }
}
