using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {
    public class Trigger : MonoBehaviour {

        public TriggerCondition[] triggerConditions = new TriggerCondition[0];
        public TriggerEffect[] triggerEffects = new TriggerEffect[0];

        private bool CheckTriggerConditions() {
            foreach (TriggerCondition c in triggerConditions)
                if (!c.IsTriggered()) return false;
            return true;
        }

        private void ExecuteTriggerEffects() {
            foreach (TriggerEffect e in triggerEffects)
                e.EffectOnTrigger();
        }

        private void FixedUpdate() {
            if (CheckTriggerConditions()) {
                ExecuteTriggerEffects();
                enabled = false;
            }
        }

    }
}
