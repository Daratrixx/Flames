using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : Animatable {


    // Use this for initialization
    void Start () {
        PrepareAnimationHelpers();
        currentAnimation = closingAnimation;
    }

    public void Open() {
        PlayAnimation(openingAnimation);
    }

    public void Close() {
        PlayAnimation(closingAnimation);
    }

    public bool isClosed { get { return IsAnimationPlaying(closingAnimation); } }
    public bool isOpened { get { return IsAnimationPlaying(openingAnimation); } }

    public override void PrepareAnimationHelpers() {
        closedAnimation.PrepareHelper();
        openingAnimation.PrepareHelper();
        openedAnimation.PrepareHelper();
        closingAnimation.PrepareHelper();
    }

    public AnimationHelper closedAnimation = new AnimationHelper() { animationName = "Closed" };
    public AnimationHelper openingAnimation = new AnimationHelper() { animationName = "Opening" };
    public AnimationHelper openedAnimation = new AnimationHelper() { animationName = "Opened" };
    public AnimationHelper closingAnimation = new AnimationHelper() { animationName = "Closing" };


}
