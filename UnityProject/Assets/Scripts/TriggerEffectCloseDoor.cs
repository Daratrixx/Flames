using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {

    
    public class TriggerEffectCloseDoor : TriggerEffect {

        public DoorController door;

        public override void EffectOnTrigger() {
            door.Close();
            Debug.Log("A door has been closed");
        }

    }
}
