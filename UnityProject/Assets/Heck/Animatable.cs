using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Animatable : MonoBehaviour {

    public Animator animatorController;
    public Transform armatureRootBone;

    protected float elapsedAnimationTime;

    void Start() {
        PrepareAnimationHelpers();
    }

    public abstract void PrepareAnimationHelpers();

    protected void StartAnimation(string animationName) {
        animatorController.Play(animationName, -1, 0);
        elapsedAnimationTime = 0;
    }
    protected void StartAnimation(int animationHash) {
        animatorController.Play(animationHash, -1, 0);
        elapsedAnimationTime = 0;
    }

    protected void CrossFadeAnimation(string animationName, float transitionDuration) {
        animatorController.CrossFade(animationName, transitionDuration);
        elapsedAnimationTime = 0;
    }

    protected void CrossFadeAnimation(int animationHash, float transitionDuration) {
        animatorController.CrossFade(animationHash, transitionDuration);
        elapsedAnimationTime = 0;
    }

    protected AnimationHelper currentAnimation = null;

    public bool IsAnimationPlaying(AnimationHelper animationHelper) {
        return currentAnimation == animationHelper;
    }

    public bool PlayAnimation(AnimationHelper animationHelper) {
        if(currentAnimation != animationHelper) {
            currentAnimation = animationHelper;
            if(currentAnimation != null) {
                CrossFadeAnimation(animationHelper.animationId, animationHelper.crossfadeDuration);
                return true;
            }
        }
        return false;
    }

    protected void RagdollOn(Transform t) {
        Rigidbody rb = t.gameObject.AddComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezePosition;
        rb.useGravity = true;
        rb.drag = 1;
        rb.angularDrag = 1;
        rb.maxAngularVelocity = 5;
        CapsuleCollider bc = t.gameObject.AddComponent<CapsuleCollider>();
        bc.radius = 0.1f;
        bc.height = 0.2f;
        bc.direction = 2;
        //bc.size = new Vector3(0.1f, 0.1f, 0.2f);
        bc.center = new Vector3(0, 0, 0.10f);
        for (int i = 0; i < t.childCount; ++i) {
            RagdollOn(t.GetChild(i));
        }
    }

    protected void RagdollOff(Transform t) {
        Destroy(t.gameObject.GetComponent<Rigidbody>());
        Destroy(t.gameObject.GetComponent<BoxCollider>());
        for (int i = 0; i < t.childCount; ++i) {
            RagdollOff(t.GetChild(i));
        }
    }

    public void TurnRagDollOn() {
        RagdollOn(armatureRootBone);
    }

    public void TurnRagDollOff() {
        RagdollOff(armatureRootBone);
    }

    public void RagDollAfterAnimation(AnimationHelper animation) {
        PlayAnimation(animation);
        var anim = animatorController.GetCurrentAnimatorStateInfo(0);
        StartCoroutine(TurnRagDollOnAfterDelay(anim.length * anim.speedMultiplier));
    }

    protected IEnumerator TurnRagDollOnAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        TurnRagDollOn();
    }

}

[Serializable]
public class AnimationHelper {

    public string animationName;
    public float crossfadeDuration = 0;

    private int ANIMATION_ID = -1;
    public int animationId {
        get { return ANIMATION_ID; }
    }

    public void PrepareHelper() {
        ANIMATION_ID = Animator.StringToHash(animationName);
    }
}