
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
/// <summary>
/// This class still doesn't follow single responsibility principle because
/// it also handles animations, we'll fix this in bootcamp 2 using Observer pattern.
/// </summary>
/// 

[RequireComponent(typeof(Animator))]
public abstract class MoveBehaviour : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float sprintMult;
    [SerializeField] protected float stopDrag;

    protected float drag;
    protected float speedMult;
    Transform playerOr;
    Vector3 directionMoving;
    float horInput;
    float verInput;
    Rigidbody rb;
    //Sub-classes can use this to check if target is reached.

    public States state;
    public enum States
    {
        walking,
        sprinting,
        crouching
    }


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerOr = transform;
        mAnimator = GetComponent<Animator>();
        PlayIdleAnimation();
    }

    public void Update()
    {
        rb.drag = drag;
        Inputs();
        StateHandler();
    }
    public void FixedUpdate()
    {
        PlayerMovement();
    }

    public virtual void SetTargetPosition(Vector3 position)
    {
        PlayMovingAnimation();
    }
    private void Inputs()
    {
        horInput = Input.GetAxisRaw("Horizontal");
        verInput = Input.GetAxisRaw("Vertical");
    }
    public virtual void Stopping()
    {
        if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        {
            drag = stopDrag;
        }
        else
        {
            drag = 1;
        }
    }
    private void PlayerMovement()
    {
        Stopping();
        directionMoving = (playerOr.forward * verInput + playerOr.right * horInput).normalized;
        rb.AddForce(directionMoving * speed * speedMult * 10f, ForceMode.Force);
    }

    public virtual void StateHandler()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            state = States.sprinting;
            speedMult = sprintMult;
        }
        else
        {
            state = States.walking;
            speedMult = 1;
        }
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
