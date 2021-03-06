﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {

    
    public class TriggerEffectOpenDoor : TriggerEffect {

        public DoorController door;

        public override void EffectOnTrigger() {
            door.Open();
            Debug.Log("A door has been opened");
        }

    }
}
