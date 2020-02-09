using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HeroesOfCode.Units
{
    public class Army : MonoBehaviour
    {
        public List<UnitStackRepresentation> unitStacks;
        private const int MaxNumberOfUnits = 5;

        private void Start() {
            if (unitStacks.Count == 0) {
                Debug.LogError("An Army must contain at least 1 unit stack");
            }
            else if (unitStacks.Count > MaxNumberOfUnits) {
                Debug.LogError($"An Army can't have more than {MaxNumberOfUnits} unit stacks.");
            }
            foreach (UnitStackRepresentation stack in unitStacks) {
                if (stack.numberOfUnits == 0) {
                    Debug.LogError($"A unit stack must contain at least one unit. Unit type: {stack.type}.");
                }
            }
        }
        public void AddUnit(UnitType unitType, uint numberOfUnits) {
            if (unitStacks.Count == MaxNumberOfUnits) return;

            unitStacks.Add(new UnitStackRepresentation() { type = unitType, numberOfUnits = numberOfUnits });
        }
    }
}
