using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using HeroesOfCode.Abilities;

namespace HeroesOfCode.UI
{
    public class IconController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public bool belongsToActiveUnit = false;
        [HideInInspector] public bool active = false;
        [SerializeField] private Image image = null;
        [SerializeField] private Image frameImage = null;
        [SerializeField] private Material grayscaleMaterial = null;
        private IDisplayIconWrapper associatedIcon;
        private IActiveAbility associatedAbility;
        private bool activatable;
        private bool isSelected;

        private void Awake() {
            EventBus.Subscribe<AbilityDeselectedEvent>(OnAbilityDeselected);
        }

        public void SetIcon(IDisplayIconWrapper iconObject) {
            gameObject.SetActive(true);
            active = true;
            associatedIcon = iconObject;
            image.sprite = associatedIcon.GetImage();
            image.material = null;
        }
        public void SetActiveAbility(IActiveAbility abilityObject) {
            associatedAbility = abilityObject;
            if (associatedAbility.CanBeUsed()) {
                image.material = null;
                activatable = true;
            }
            else {
                image.material = grayscaleMaterial;
            }
        }

        public void OnPointerEnter(PointerEventData eventData) {
            EventBus.FireEvent<IconUnderMouseEvent>(new IconUnderMouseEvent() { Icon = associatedIcon, IconBelongsToActiveUnit = belongsToActiveUnit });
        }
        public void OnPointerExit(PointerEventData eventData) {
            EventBus.FireEvent<MouseLeavesIconEvent>(new MouseLeavesIconEvent() { IconBelongsToActiveUnit = belongsToActiveUnit });
        }

        public void OnPointerClick(PointerEventData eventData) {
            if (eventData.button == PointerEventData.InputButton.Left && belongsToActiveUnit && activatable) {
                if (isSelected) {
                    EventBus.FireEvent<AbilityDeselectedEvent>();
                }
                else if (associatedAbility.CanBeUsed()) {
                    frameImage.gameObject.SetActive(true);
                    isSelected = true;
                    EventBus.FireEvent<AbilitySelectedEvent>(new AbilitySelectedEvent() { Ability = associatedAbility });
                }
            }
        }
        private void OnAbilityDeselected(AbilityDeselectedEvent customEvent) {
            if (isSelected) {
                Deselect();
            }
        }

        private void Deselect() {
            if (!isSelected) return;

            isSelected = false;
            frameImage.gameObject.SetActive(false);
        }
        public void DisableSelf() {
            activatable = false;
            active = false;
            associatedAbility = null;
            associatedIcon = null;
            gameObject.SetActive(false);
        }
    }
}
