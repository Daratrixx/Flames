using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;


public class PlayerController : MonoBehaviour {

    public PlayerInput inputs;

    public Character character;
    public Camera playerCamera;

    public Vector2 cameraAngleLimits = new Vector2(35,-35);
    public float cameraAngle = 20;
    public float cameraRotation = -90;
    public float cameraDistance = 4;
    public Vector3 cameraOffset = Vector3.up * 2;
    public float cameraFollowSpeed = 5;
    public const int cameraLayer = 1 << 10;
    public float cameraCollisionOffset = 0.5f;

    public bool gameCursor = false;

    private void Start() {
        if (!Application.isEditor) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        ResetCamera();
    }

    public void SetGameCursor(bool value) {
        if (value) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void LateUpdate() {
        UpdateCamera(cameraFollowSpeed * Time.deltaTime);
    }

    private Vector3 lastAnchor;
    private Vector3 cameraAnchor {
        get {
            if (character != null) return lastAnchor = character.transform.position + cameraOffset;
            return lastAnchor;
        }
    }

    public void UpdateCamera(float deltaTime) {
        Vector3 direction;
        direction = new Vector3(
            Mathf.Cos(cameraAngle * Mathf.Deg2Rad) * Mathf.Sin(cameraRotation * Mathf.Deg2Rad),
            Mathf.Sin(cameraAngle * Mathf.Deg2Rad),
            Mathf.Cos(cameraAngle * Mathf.Deg2Rad) * Mathf.Cos(cameraRotation * Mathf.Deg2Rad));

        playerCamera.transform.forward = Vector3.Lerp(playerCamera.transform.forward, direction, deltaTime);
        RaycastHit cameraAdjutCast;
        if (Physics.Raycast(cameraAnchor, -direction, out cameraAdjutCast, cameraDistance, cameraLayer)) {
            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position,
                cameraAnchor - direction * (cameraAdjutCast.distance - cameraCollisionOffset),
                deltaTime);
        } else {
            playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position,
                cameraAnchor - direction * cameraDistance,
                deltaTime);
        }
    }
    public void ResetCamera() {
        cameraRotation = character.transform.eulerAngles.y;
        Vector3 direction = new Vector3(Mathf.Cos(cameraAngle * Mathf.Deg2Rad) * Mathf.Sin(cameraRotation * Mathf.Deg2Rad),
            Mathf.Sin(cameraAngle * Mathf.Deg2Rad),
            Mathf.Cos(cameraAngle * Mathf.Deg2Rad) * Mathf.Cos(cameraRotation * Mathf.Deg2Rad));

        playerCamera.transform.forward = direction;
        RaycastHit cameraAdjutCast;
        if (Physics.Raycast(cameraAnchor, -direction, out cameraAdjutCast, cameraDistance, cameraLayer)) {
            playerCamera.transform.position = cameraAnchor - direction * (cameraAdjutCast.distance - cameraCollisionOffset);
        } else {
            playerCamera.transform.position = cameraAnchor - direction * cameraDistance;
        }
    }

    public void FixedUpdate() {
        try {
        } catch (MissingReferenceException) {
            character = null;
        }
    }
    public void Update() {
        try {
            ControllCamera();
            CheckInputs();
        } catch (MissingReferenceException) {
            character = null;
        }
    }

    public void ControllCamera() {
        Vector2 camera = inputs.GetCameraAxis();
        cameraAngle += camera.y;
        cameraRotation += camera.x;
        //cameraAngle = Mathf.Clamp(cameraAngle, cameraAngleLimits.x, cameraAngleLimits.y);
        if (cameraAngle > cameraAngleLimits.x) cameraAngle = cameraAngleLimits.x;
        if (cameraAngle < cameraAngleLimits.y) cameraAngle = cameraAngleLimits.y;
        if (cameraRotation >= 360)
            cameraRotation -= 360;
        if (cameraRotation < 0)
            cameraRotation += 360;
    }
    
    public void CheckInputs() {
        if(inputs.SwapPressed()) {
            GameManager.SwapCharacters();
            return;
        }
        if (character == null) return;
        Vector2 movement = inputs.GetMovementAxis();
        float forWalk = movement.y;
        float sideWalk = movement.x;
        if (movement.magnitude > 0.001f) {
            Vector3 direction = playerCamera.transform.forward * forWalk + playerCamera.transform.right * sideWalk;
            direction.y = 0;
            if(inputs.JumpPressed()) {
                character.Jump();
            } else if (inputs.RunDown()) {
                character.Run(direction.normalized);
            } else {
                character.Walk(direction.normalized);
            }
        } else {
            if (inputs.JumpPressed()) {
                character.Jump();
            } else {
                character.NoMove();
            }
        }
    }

}
