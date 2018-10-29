using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour {

    public static List<Character> characters = new List<Character>();

    public Animator animatorController;
    public PlayerController controller;
    public AudioSource audioSource;
    public Collider characterCollider;

    public float rotationSpeed = 7.5f;
    public float walkSpeed = 3;
    public float runSpeed = 6;

    private float elapsedAnimationTime;

    public const float gravitySpeed = 20;
    public const float gravityOffset = 0.05f;
    public const int obstaclesCollisionLayer = (1 << 10) + (1 << 11);
    public const float jumpGravityFactor = 0.40f;
    public const float jumpVelocity = 5;
    public Vector3 velocity;
    public Vector3 movement;
    private float gravityFactor = 1;

    public Vector3 groundPosition {
        get {
            RaycastHit info;
            if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out info, obstaclesCollisionLayer))
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
    public bool isGrounded;
    public bool isFalling;
    public bool isJumping;

    public void Walk(Vector3 direction) {
        if (isGrounded) {
            if (!isMoving || isRunning) {
                isRunning = false;
                isMoving = true;
                CrossFadeAnimation("MoveForward", 0.3f);
            }
        }
        transform.position += direction * walkSpeed * Time.deltaTime;
        //movement = direction * walkSpeed * Time.deltaTime;
        direction = Vector3.Slerp(forward, direction, rotationSpeed * Time.deltaTime);
        direction.y = 0;
        forward = direction.normalized;
    }
    public void Strafe(Vector3 aim, Vector3 movement) {
        if (isGrounded) {
            if (!isMoving) {
                isMoving = true;
                CrossFadeAnimation("MoveForward", 0.3f);
            }
        }
        transform.position += transform.rotation * movement * walkSpeed * Time.deltaTime;
        //movement = transform.rotation * movement * walkSpeed * Time.deltaTime;
        aim = Vector3.Slerp(forward, aim, rotationSpeed * Time.deltaTime);
        aim.y = 0;
        forward = aim.normalized;
    }
    public void Run(Vector3 direction) {
        if (isGrounded) {
            if (!isMoving || !isRunning) {
                isRunning = true;
                isMoving = true;
                CrossFadeAnimation("MoveForward", 0.3f);
            }
        }
        transform.position += direction * runSpeed * Time.deltaTime;
        //movement = direction * walkSpeed * Time.deltaTime;
        direction = Vector3.Slerp(forward, direction, rotationSpeed * Time.deltaTime);
        direction.y = 0;
        forward = direction.normalized;
    }
    public void Jump() {
        if (isGrounded) {
            if (isMoving) {
                isMoving = false;
                isRunning = false;
            }
            isJumping = true;
            velocity.y = jumpVelocity;
            gravityFactor = jumpGravityFactor;
            CrossFadeAnimation("Jumping", 0.3f);
            //CrossFadeAnimation("Stand", 0.3f);
        }
    }
    public void Rotate(Vector3 direction) {
        direction = Vector3.Slerp(forward, direction, rotationSpeed * Time.deltaTime);
        direction.y = 0;
        forward = direction.normalized;
        NoMove();
    }
    public void NoMove() {
        if (isGrounded) {
            if (isMoving) {
                isMoving = false;
                isRunning = false;
                BackToStandAnimation();
            }
        }
        //movement = Vector3.zero;
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
        // detect end of jump upward movement
        if (isJumping && velocity.y <= 0) {
            isJumping = false;
        }
        if (!isFalling && velocity.y < 0) {
            isFalling = true;
            gravityFactor = 1;
            CrossFadeAnimation("Falling", 0.3f);
        }
        float acceleration = Time.deltaTime * gravitySpeed * gravityFactor;
        velocity.y -= acceleration;
        float gravityEffect = velocity.y * Time.deltaTime;
        RaycastHit hitInfo;
        if (gravityEffect < 0) {
            if (Physics.Raycast(transform.position + Vector3.up*0.5f, -Vector3.up, out hitInfo, -gravityEffect + 0.5f + gravityOffset, obstaclesCollisionLayer)) {
                transform.position = hitInfo.point + Vector3.up * gravityOffset;
                velocity.y = 0;
                if (isFalling) {
                    isFalling = false;
                }
                if (!isGrounded) {
                    isGrounded = true;
                    BackToStandAnimation();
                }
            } else {
                transform.position += Vector3.up * gravityEffect;
                if (isGrounded) isGrounded = false;
            }
        } else if (gravityEffect > 0) {
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.up, out hitInfo, gravityEffect + -0.5f + gravityOffset, obstaclesCollisionLayer)) {
                transform.position = hitInfo.point - Vector3.up * gravityOffset;
                velocity.y = 0;
                if (isJumping) isJumping = false;
            } else {
                transform.position += Vector3.up * gravityEffect;
                if (isGrounded) isGrounded = false;
            }
        }
    }

    /*void Update() {
        // detect end of jump upward movement
        if (isJumping && velocity.y <= 0) {
            isJumping = false;
        }
        if (!isFalling && velocity.y < 0) {
            isFalling = true;
            gravityFactor = 1;
            CrossFadeAnimation("Falling", 0.3f);
        }
        if (isGrounded) isGrounded = false;
        float acceleration = Time.deltaTime * gravitySpeed * gravityFactor;
        velocity.y -= acceleration;
        Vector3 direction = movement + velocity * Time.deltaTime;
        RaycastHit[] hits = CastColliderAll(characterCollider, direction);
        if (hits != null && hits.Length > 0) {
            float minDistance = -1;
            RaycastHit minRh;
            foreach (RaycastHit rh in hits) {
                if (minDistance == -1 || rh.distance < minDistance) {
                    minDistance = rh.distance;
                    minRh = rh;
                }
                direction = direction.normalized * (minDistance - 0.001f);
                CharacterHitObstacle(rh);
            }
        }
        transform.position += direction;

        movement = Vector3.zero;
    }*/

    void FixedUpdate() {
        elapsedAnimationTime += Time.fixedDeltaTime;
    }




    private void CharacterHitObstacle(RaycastHit rh) {
        Vector3 center = characterCollider.transform.position;
        if (characterCollider is BoxCollider) {
            center += characterCollider.transform.rotation * Vector3.Scale(((BoxCollider)characterCollider).center, characterCollider.transform.lossyScale);
        } else if (characterCollider is CapsuleCollider) {
            center += characterCollider.transform.rotation * Vector3.Scale(((CapsuleCollider)characterCollider).center, characterCollider.transform.lossyScale);
        }
        float upDot = Vector3.Dot(Vector3.up, (rh.point - center).normalized);
        float leftDot = Vector3.Dot(Vector3.left, (rh.point - center).normalized);
        float forwardDot = Vector3.Dot(Vector3.forward, (rh.point - center).normalized);
        if (Math.Abs(upDot) > Math.Abs(leftDot) && Math.Abs(upDot) > Math.Abs(forwardDot)) {
            velocity.y = 0;
            if (upDot < 0) {
                if (isFalling) isFalling = false;
                isGrounded = true;
            }
        }

    }

    public static RaycastHit[] CastColliderAll(Collider c, Vector3 direction) {
        RaycastHit[] hits = null;
        if (c is BoxCollider) {
            BoxCollider bc = (BoxCollider)c;
            hits = Physics.BoxCastAll(bc.transform.position + bc.transform.rotation * Vector3.Scale(bc.center, bc.transform.lossyScale),
                Vector3.Scale(bc.size / 2, bc.transform.lossyScale),
                direction.normalized,
                bc.transform.rotation,
                direction.magnitude,
                obstaclesCollisionLayer);
        } else if (c is CapsuleCollider) {
            CapsuleCollider cc = (CapsuleCollider)c;
            Vector3 height = new Vector3(0, cc.height / 2, 0);
            Vector3 start = cc.transform.position + cc.transform.rotation * (cc.center - height);
            Vector3 end = start + cc.transform.rotation * height * 2;
            hits = Physics.CapsuleCastAll(start,
                end,
                cc.radius,
                direction.normalized,
                direction.magnitude,
                obstaclesCollisionLayer);
        }
        return hits;
    }

}

