using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Input", menuName = "PlayerInput")]
public class PlayerInput : ScriptableObject {

    public Vector2 GetMovementAxis() {
        float x = 0, y = 0;
        if (Input.GetKey(forward)) y += 1;
        if (Input.GetKey(backward)) y -= 1;
        if (Input.GetKey(leftward)) x -= 1;
        if (Input.GetKey(rightward)) x += 1;
        return new Vector2(x + Input.GetAxisRaw("Joystick M X"), y + Input.GetAxisRaw("Joystick M Y"));
    }
    public Vector2 GetCameraAxis() {
        return new Vector2(Input.GetAxisRaw("Mouse X") + Input.GetAxisRaw("Joystick C X"),
            Input.GetAxisRaw("Mouse Y") + Input.GetAxisRaw("Joystick C Y"));
    }

    public bool LeftLightPressed() { return Input.GetKeyDown(leftLight); }
    public bool LeftHeavyPressed() { return Input.GetKeyDown(leftHeavy); }
    public bool RightLightPressed() { return Input.GetKeyDown(rightLight); }
    public bool RightHeavyPressed() { return Input.GetKeyDown(rightHeavy); }

    public bool LeftLightReleased() { return Input.GetKeyUp(leftLight); }
    public bool LeftHeavyReleased() { return Input.GetKeyUp(leftHeavy); }
    public bool RightLightReleased() { return Input.GetKeyUp(rightLight); }
    public bool RightHeavyReleased() { return Input.GetKeyUp(rightHeavy); }

    public bool Spell0Pressed() { return Input.GetKeyDown(spell0); }
    public bool Spell1Pressed() { return Input.GetKeyDown(spell1); }
    public bool Spell2Pressed() { return Input.GetKeyDown(spell2); }
    public bool Spell3Pressed() { return Input.GetKeyDown(spell3); }

    public bool Spell0Released() { return Input.GetKeyUp(spell0); }
    public bool Spell1Released() { return Input.GetKeyUp(spell1); }
    public bool Spell2Released() { return Input.GetKeyUp(spell2); }
    public bool Spell3Released() { return Input.GetKeyUp(spell3); }

    public bool RollPressed() { return Input.GetKeyDown(roll); }
    public bool LockOnPressed() { return Input.GetKeyDown(lockOn); }
    public bool RunDown() { return Input.GetKey(run); }
    public bool JumpPressed() { return Input.GetKeyDown(jump); }

    public bool SwapPressed() { return Input.GetKeyDown(swap); }

    public KeyCode forward = KeyCode.Z;
    public KeyCode leftward = KeyCode.Q;
    public KeyCode backward = KeyCode.S;
    public KeyCode rightward = KeyCode.D;

    public KeyCode leftLight = KeyCode.LeftShift;
    public KeyCode leftHeavy = KeyCode.Tab;
    public KeyCode rightLight = KeyCode.Mouse0;
    public KeyCode rightHeavy = KeyCode.Mouse1;

    public KeyCode spell0 = KeyCode.Alpha1;
    public KeyCode spell1 = KeyCode.Alpha2;
    public KeyCode spell2 = KeyCode.Alpha3;
    public KeyCode spell3 = KeyCode.Alpha4;

    public KeyCode jump = KeyCode.Space;
    public KeyCode run = KeyCode.Space;
    public KeyCode roll = KeyCode.Space;
    public KeyCode lockOn = KeyCode.Mouse2;

    public KeyCode swap = KeyCode.Tab;

    public void SetLayoutZQSD() {
        forward = KeyCode.Z;
        leftward = KeyCode.Q;
        backward = KeyCode.S;
        rightward = KeyCode.D;
    }
    public void SetLayoutWASD() {
        forward = KeyCode.W;
        leftward = KeyCode.A;
        backward = KeyCode.S;
        rightward = KeyCode.D;
    }

}
