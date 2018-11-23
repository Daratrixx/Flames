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
        //rb.constraints = RigidbodyConstraints.FreezePosition;
        rb.useGravity = true;
        rb.drag = 1;
        rb.angularDrag = 1;
        rb.angularDrag = 1;
        //rb.maxAngularVelocity = 5;
        rb.mass = 1;
        Rigidbody parentRb;
        if ((parentRb = t.parent.GetComponent<Rigidbody>()) != null) {
            HingeJoint j = t.gameObject.AddComponent<HingeJoint>();
            j.autoConfigureConnectedAnchor = true;
            j.connectedBody = parentRb;
            //j.enableCollision = true;
            //j.connectedAnchor = Vector3.Scale(t.localPosition, t.lossyScale);
            j.axis = new Vector3(1, 1, 1);
        }
        if (t.GetComponent<Collider>() != null) return;
        CapsuleCollider bc = t.gameObject.AddComponent<CapsuleCollider>();
        bc.radius = 0.15f;
        bc.height = 0.6f;// Vector3.Scale(t.localPosition, t.lossyScale).magnitude;
        bc.direction = 0;
        bc.center = new Vector3(-bc.height / 2, 0, 0);
        for (int i = 0; i < t.childCount; ++i) {
            RagdollOn(t.GetChild(i));
        }
    }

    protected void RagdollOff(Transform t) {
        Destroy(t.gameObject.GetComponent<Rigidbody>());
        Destroy(t.gameObject.GetComponent<BoxCollider>());
        Destroy(t.gameObject.GetComponent<HingeJoint>());
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

    public void RagDollOnAfterAnimation(AnimationHelper animation) {
        //PlayAnimation(animation);
        //var anim = animatorController.GetCurrentAnimatorStateInfo(0);
        //StartCoroutine(TurnRagDollOnAfterDelay(anim.length * anim.speedMultiplier));
        currentAnimation = animation;
        animatorController.enabled = false;
        StartCoroutine(TurnRagDollOnAfterDelay(0));
    }

    public void RagDollOffAfterAnimation(AnimationHelper animation) {
        //PlayAnimation(animation);
        //var anim = animatorController.GetCurrentAnimatorStateInfo(0);
        //StartCoroutine(TurnRagDollOnAfterDelay(anim.length * anim.speedMultiplier));
        currentAnimation = animation;
        animatorController.enabled = true; ;
        StartCoroutine(TurnRagDollOffAfterDelay(0));
    }

    protected IEnumerator TurnRagDollOnAfterDelay(float delay) {
        TurnRagDollOn();
        yield return new WaitForSeconds(delay);
        //animatorController.StopPlayback();
    }

    protected IEnumerator TurnRagDollOffAfterDelay(float delay) {
        TurnRagDollOff();
        yield return new WaitForSeconds(delay);
        //animatorController.StartPlayback();
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