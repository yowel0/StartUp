
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
/// <summary>
/// This class still doesn't follow single responsibility principle because
/// it also handles animations, we'll fix this in bootcamp 2 using Observer pattern.
/// </summary>
[RequireComponent(typeof(Animator))]
public abstract class MoveBehaviour : MonoBehaviour
{
    //Sub-classes can use this to check if target is reached.


    public void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {

        }
    }

    protected virtual void Start()
    {
        mAnimator = GetComponent<Animator>();
        PlayIdleAnimation();
    }

    public virtual void SetTargetPosition(Vector3 position)
    {
        PlayMovingAnimation();
    }


    #region "animation related fields"
    [SerializeField]
    private Animator mAnimator;
    //If idling animation is already playing, don't play it again.
    private bool idleAnimationPlaying = false;
    //If moving animation is already playing, don't play it again.
    private bool moveAnimationPlaying = false;
    //Sub-classes can use the following functions to play animations accordingly
    protected void PlayMovingAnimation()
    {
        idleAnimationPlaying = false;
        if (!moveAnimationPlaying)
        {
            mAnimator.Play("Move");
            moveAnimationPlaying = true;
        }
    }
    protected void PlayIdleAnimation()
    {
        moveAnimationPlaying = false;
        if (!idleAnimationPlaying)
        {
            mAnimator.Play("Idle");
            idleAnimationPlaying = true;
        }
    }
    #endregion
}
