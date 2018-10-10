﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour {

    public static List<Character> characters = new List<Character>();

    public Animator animatorController;
    public PlayerController controller;
    public AudioSource audioSource;

    public float rotationSpeed = 7.5f;
    public float walkSpeed = 3;
    public float runSpeed = 6;

    private float elapsedAnimationTime;

    private float gravitySpeed = 15;
    public const float gravityOffset = 0.05f;
    public const int gravityCollisionLayer = 1 << 10;

    public Vector3 groundPosition {
        get {
            RaycastHit info;
            if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out info, gravityCollisionLayer))
                return info.point;
            return transform.position;
        }
    }

    void Start() {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
        if (animatorController == null)
            animatorController = GetComponent<Animator>();
        if (animatorController == null)
            animatorController = gameObject.AddComponent<Animator>();
        if (controller == null)
            controller = GetComponent<PlayerController>();
        if (controller == null)
            controller = gameObject.AddComponent<PlayerController>();
        characters.Add(this);
    }

    public Vector3 forward {
        get { return transform.forward; }
        set { transform.forward = value; }
    }
    public Vector3 right {
        get { return transform.right; }
    }
    public Vector3 up {
        get { return transform.up; }
    }
    
    public bool isMoving;
    public bool isRunning;
    public void Walk(Vector3 direction) {
        if (!isMoving || isRunning) {
            isRunning = false;
            isMoving = true;
            CrossFadeAnimation("MoveForward", 0.3f);
        }
        transform.position += direction * walkSpeed * Time.deltaTime;
        direction = Vector3.Slerp(forward, direction, rotationSpeed * Time.deltaTime);
        direction.y = 0;
        forward = direction.normalized;
    }
    public void Strafe(Vector3 aim, Vector3 movement) {
        if (!isMoving) {
            isMoving = true;
            CrossFadeAnimation("MoveForward", 0.3f);
        }
        transform.position += transform.rotation * movement * walkSpeed * Time.deltaTime;
        aim = Vector3.Slerp(forward, aim, rotationSpeed * Time.deltaTime);
        aim.y = 0;
        forward = aim.normalized;
    }
    public void Run(Vector3 direction) {
        if (!isMoving || !isRunning) {
            isRunning = true;
            isMoving = true;
            CrossFadeAnimation("Run", 0.3f);
        }
        transform.position += direction * runSpeed * Time.deltaTime;
        direction = Vector3.Slerp(forward, direction, rotationSpeed * Time.deltaTime);
        direction.y = 0;
        forward = direction.normalized;
    }
    public void Rotate(Vector3 direction) {
        direction = Vector3.Slerp(forward, direction, rotationSpeed * Time.deltaTime);
        direction.y = 0;
        forward = direction.normalized;
        NoMove();
    }
    public void NoMove() {
        if (isMoving) {
            isMoving = false;
            isRunning = false;
            BackToStandAnimation();
        }
    }


    #region ANIMATION_CONTROLLS

    private void StartAnimation(string animationName) {
        //animatorController.CrossFade(animationName, 0.1f);
        animatorController.Play(animationName, -1, 0);
        elapsedAnimationTime = 0;
    }

    private void CrossFadeAnimation(string animationName, float transitionDuration) {
        animatorController.CrossFade(animationName, transitionDuration);
        elapsedAnimationTime = 0;
    }

    private void BackToStandAnimation() {
        StartAnimation("Stand");
    }

    #endregion

    #region MOVE_ANIMATION_CONTROLLS

    public void PlayAudioClip(AudioClip clip) {
        audioSource.PlayOneShot(clip);
    }

    public void ApplyForce(Vector3 force) {
        transform.position += force;
    }
    public void ApplyDisplacement(Vector3 displacement) {
        transform.position += right * displacement.x +
            up * displacement.y +
            forward * displacement.z;
    }

    #endregion

    void Update() {
        float gravityEffect = Time.deltaTime * gravitySpeed;
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position + Vector3.up, -Vector3.up, out hitInfo, gravityEffect + 1, gravityCollisionLayer)) {
            transform.position = hitInfo.point + Vector3.up * gravityOffset;
        } else {
            transform.position -= Vector3.up * gravityEffect;
        }
    }

    void FixedUpdate() {
        elapsedAnimationTime += Time.fixedDeltaTime;
    }
}
