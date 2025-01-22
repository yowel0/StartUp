
using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using FMODUnity;
using FMOD.Studio;

/// <summary>
/// This class still doesn't follow single responsibility principle because
/// it also handles animations, we'll fix this in bootcamp 2 using Observer pattern.
/// </summary>
/// 

[RequireComponent(typeof(Animator))]
public abstract class MoveBehaviour : MonoBehaviour
{
    public float speed;
    [SerializeField] float sprintMult;
    [SerializeField] LayerMask whatIsGround;

    [SerializeField] float maxSlopeAngle;

    [SerializeField]
    private EventReference footstepEvent;
    
    private float footstepDelay = 0.5f; // Delay between footstep sounds
    private float lastFootstepTime = 0f;


    private RaycastHit slopeHit;
    

    public bool grounded;
    [SerializeField] bool onSlope;
    protected bool checkForGround = true;
    protected float drag;
    protected float speedMult;
    Transform playerOr;
    Vector3 directionMoving;
    float horInput;
    float verInput;
    protected Rigidbody rb;
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
        if (checkForGround)
        {
            grounded = Physics.Raycast(transform.position, Vector3.down, transform.localScale.y + 0.2f, whatIsGround);
        }
        SpeedControl();
        Inputs();
        StateHandler();
        if (!grounded && !OnSlope())
        {
            rb.AddForce(Vector3.down, ForceMode.Impulse);
        }
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
    private void PlayerMovement()
    {
        directionMoving = (playerOr.forward * verInput + playerOr.right * horInput).normalized;
        rb.AddForce(directionMoving * speed * speedMult * 10f, ForceMode.Force);
        if (directionMoving.magnitude == 0)
        {
            rb.isKinematic = true;
           
           
        }
        else
        {
            rb.isKinematic = false;
            PlayFootstepSound();

            


        }
        if (OnSlope())
        {
            rb.AddForce(GetSlopeDir() * speed * speedMult * 20f, ForceMode.Force);
        }
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            rb.velocity = new Vector3(limitedVel.x,rb.velocity.y, limitedVel.z);
        }
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, transform.localScale.y + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            onSlope = angle < maxSlopeAngle && angle != 0;
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeDir()
    {
        return Vector3.ProjectOnPlane(directionMoving, slopeHit.normal).normalized;
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


    private void PlayFootstepSound()
    {
        if (Time.time >= lastFootstepTime + footstepDelay && directionMoving.magnitude > 0)
        {
            RuntimeManager.PlayOneShot(footstepEvent, transform.position);
            lastFootstepTime = Time.time;
        }

    }
  
    private void OnDestroy()
    {
        // Ensure the event instance is released when the object is destroyed
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
