using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {
    public class TriggerEffectOpenDoor : TriggerEffect {

        public override void EffectOnTrigger() {
            Debug.Log("A door has been openned");
        }

    }
}
