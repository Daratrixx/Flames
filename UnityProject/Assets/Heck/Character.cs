using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Animatable {

    public static List<Character> characters = new List<Character>();

    [SerializeField]
    public CharacterController characterController;
    public AudioSource audioSource;

    public float rotationSpeed = 7.5f;
    public float walkSpeed = 3;
    public float runSpeed = 6;
    public AnimationCurve jumpForceCurve;
    public float jumpMultiplier = 5;

    public const float jumpGravityFactor = 0.40f;

    public Vector3 velocity;
    public Vector3 inputs;

    void Start() {
        PrepareAnimationHelpers();
        characters.Add(this);
        CombatUnit unit = GetComponent<CombatUnit>();
        if (unit != null) {
            unit.RegisterDeathListener(Die);
        }
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
            if (!isMoving || isRunning || !IsAnimationPlaying(walkAnimation)) {
                isRunning = false;
                isMoving = true;
                PlayAnimation(walkAnimation);
            }
        }
        inputs = direction * walkSpeed;
        direction = Vector3.Slerp(forward, direction, rotationSpeed * Time.deltaTime);
        direction.y = 0;
        forward = direction.normalized;
    }
    public void Strafe(Vector3 aim, Vector3 movement) {
        if (isGrounded) {
            if (!isMoving || !IsAnimationPlaying(walkAnimation)) {
                isMoving = true;
                PlayAnimation(walkAnimation);
            }
        }
        inputs = transform.rotation * movement * walkSpeed;
        aim = Vector3.Slerp(forward, aim, rotationSpeed * Time.deltaTime);
        aim.y = 0;
        forward = aim.normalized;
    }
    public void Run(Vector3 direction) {
        if (isGrounded) {
            if (!isMoving || !isRunning || !IsAnimationPlaying(runAnimation)) {
                isRunning = true;
                isMoving = true;
                PlayAnimation(runAnimation);
            }
        }
        inputs = direction * runSpeed;
        direction = Vector3.Slerp(forward, direction, rotationSpeed * Time.deltaTime);
        direction.y = 0;
        forward = direction.normalized;
    }
    public void Jump() {
        if (isGrounded || !isJumping) {
            isJumping = true;
            StartCoroutine(ApplyJump());
            PlayAnimation(jumpAnimation);
        }
    }

    private IEnumerator ApplyJump() {
        isJumping = true;
        isGrounded = false;
        float elapsedTime = 0;
        Vector3 jump = Vector3.up * jumpMultiplier;
        Vector3 gravityCompensation = -Physics.gravity * jumpGravityFactor;
        float slopeLimit = characterController.slopeLimit;
        characterController.slopeLimit = 90;
        while (!isGrounded && characterController.collisionFlags != CollisionFlags.Above) {
            elapsedTime += Time.deltaTime;
            characterController.Move((jump * jumpForceCurve.Evaluate(elapsedTime) + gravityCompensation) * Time.deltaTime);
            if (isFalling) {
                gravityCompensation = Vector3.zero;
            }
            yield return null;
        }
        characterController.slopeLimit = slopeLimit;
        isJumping = false;
    }

    public void Rotate(Vector3 direction) {
        direction = Vector3.Slerp(forward, direction, rotationSpeed * Time.deltaTime);
        direction.y = 0;
        forward = direction.normalized;
        NoMove();
    }
    public void NoMove() {
        if (isGrounded) {
            if (isMoving || IsAnimationPlaying(fallAnimation)) {
                isMoving = false;
                isRunning = false;
                //BackToStandAnimation();
                PlayAnimation(standAnimation);
            }
        }
        inputs = Vector3.zero;
    }
    public void Die() {
        //Destroy(characterController);
        //characterController.enabled = false;
        RagDollOnAfterAnimation(deathAnimation);
        inputs = Vector3.zero;
        //Destroy(this);
    }

    #region MOVE_ANIMATION_CONTROLLS

    public void PlayAudioClip(AudioClip clip) {
        audioSource.PlayOneShot(clip);
    }

    #endregion

    protected Vector3 lastPosition;
    void Update() {
        Vector3 currentPosition = transform.position;
        if (IsAnimationPlaying(deathAnimation)) return;
        characterController.SimpleMove(inputs);
        isGrounded = characterController.isGrounded;
        if (isGrounded) {
            if (isFalling) isFalling = false;
        } else {
            if (!isFalling && lastPosition.y > currentPosition.y) {
                isFalling = true;
                PlayAnimation(fallAnimation);
            }
        }

        // slop/steps compensation
        /*if (!isGrounded && !isJumping) {
            RaycastHit info;
            if (Physics.Raycast(transform.position, Vector3.down, out info, characterController.stepOffset, 1 << 10))
                characterController.Move(Vector3.down * info.distance);
        }*/

        lastPosition = currentPosition;
        //inputs = Vector3.zero;
    }

    private void LateUpdate() {
        isGrounded = characterController.isGrounded;
    }

    void FixedUpdate() {
        elapsedAnimationTime += Time.fixedDeltaTime;
    }

    void OnControllerColliderHit(ControllerColliderHit hit) {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic) {
            Vector3 velocity = lastPosition - transform.position;
            velocity.x = 0;
            velocity.z = 0;
            body.position += inputs * Time.deltaTime; //velocity;
            body.velocity += velocity;
        }
    }

    /*
        1-Inputs => create input movement vector
        2-Velocity => update velocity depending on friction, gravity, and movement (inputs)
        3-Sweep => Sweep using calculated velocity
        4-Update => update position depending on the sweep results
    */

    /*
public void UpdateVelocity() {
    // 2 - Velocity => update velocity depending on friction, gravity, and movement(inputs)
    // a) friction
    if (isGrounded)
        velocity.Scale(new Vector3(0.90f, 1, 0.90f));
    else
        velocity.Scale(new Vector3(0.99f, 1, 0.99f));
    // b) gravity
    velocity.y -= Time.deltaTime * gravitySpeed * gravityFactor;
    // c) movement
    inputs.y = 0;
    velocity += inputs * Time.deltaTime;
    inputs = Vector3.zero;
}
public void UpdateCharacterPosition() {

    // 3 - Sweep => Sweep using calculated velocity
    Rigidbody body = GetComponent<Rigidbody>();
    RaycastHit info;


    // TODO: OPTIMIZE
    if (body.SweepTest(velocity.normalized, out info, velocity.magnitude * Time.deltaTime + gravityOffset)) { //4 - Update => update position depending on the sweep results
        // we collided in some shit
        if(info.distance > gravityOffset)
            transform.position += velocity.normalized * Time.deltaTime * (info.distance - gravityOffset); // scale initial movement with the right distance
        float dot = Vector3.Dot(info.normal, Vector3.up);
        if (dot > 0.35) { // this is ground
            if (!isGrounded) {
                isGrounded = true;
            }
            if (isJumping) isJumping = false;
            if (isFalling) isFalling = false;
            velocity.y = 0;
        } else {
            if (isGrounded) {
                isGrounded = false;
            }
            if (dot < -0.35) { // this is ceiling
                if (isJumping) isJumping = false;
                if (isFalling) isFalling = false;
                velocity.y = 0;
            } else { // this is shit
                if (isJumping && velocity.y <= 0) {
                    isJumping = false;
                    if (isFalling) isFalling = false; // make sure the transition to fall animation occurs
                }
                if (!isFalling && velocity.y < 0) {
                    isFalling = true;
                    if (isMoving) isMoving = false;
                    if (isRunning) isRunning = false;
                    gravityFactor = 1;
                    //CrossFadeAnimation("Falling", 0.3f);
                    PlayAnimation(fallAnimation);
                }
            }
        }
        //c.transform.position += c.movement;
    } else { // nothing collided, we're just falling or something
        transform.position += velocity * Time.deltaTime; // do the full motion
        if (isGrounded) isGrounded = false;
        if (isJumping && velocity.y <= 0) {
            isJumping = false;
            if (isFalling) isFalling = false; // make sure the transition to fall animation occurs
        }
        if (!isFalling && velocity.y < 0) {
            isFalling = true;
            if (isMoving) isMoving = false;
            if (isRunning) isRunning = false;
            gravityFactor = 1;
            //CrossFadeAnimation("Falling", 0.3f);
            PlayAnimation(fallAnimation);
        }
    }
}
*/

    public override void PrepareAnimationHelpers() {
        standAnimation.PrepareHelper();
        walkAnimation.PrepareHelper();
        runAnimation.PrepareHelper();
        jumpAnimation.PrepareHelper();
        fallAnimation.PrepareHelper();
        deathAnimation.PrepareHelper();
    }

    public AnimationHelper standAnimation = new AnimationHelper() { animationName = "Stand", crossfadeDuration = 0.3f };
    public AnimationHelper walkAnimation = new AnimationHelper() { animationName = "MoveForward", crossfadeDuration = 0.3f };
    public AnimationHelper runAnimation = new AnimationHelper() { animationName = "Running", crossfadeDuration = 0.3f };
    public AnimationHelper jumpAnimation = new AnimationHelper() { animationName = "Jumping", crossfadeDuration = 0.5f };
    public AnimationHelper fallAnimation = new AnimationHelper() { animationName = "Falling", crossfadeDuration = 2 };
    public AnimationHelper deathAnimation = new AnimationHelper() { animationName = "Death", crossfadeDuration = 0.3f };

}