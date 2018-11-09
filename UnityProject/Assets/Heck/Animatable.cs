using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Animatable : MonoBehaviour {

    public Animator animatorController;

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