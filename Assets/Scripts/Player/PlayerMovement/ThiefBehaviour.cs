using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class ThiefBehaviour : MoveBehaviour
{
    [SerializeField] float crouchSpeed;
    [SerializeField]
    float CrouchMultiplier = 0.5f;

    Camera cam;
    [SerializeField] CapsuleCollider caps1;
    [SerializeField]CapsuleCollider caps2;

    public bool grabbed = false;

    bool crouching = false;

    float oldSpeed;

    public void Start()
    {
        cam = Camera.main;
        oldSpeed = speed;
        base.Start();
    }
    public override void StateHandler()
    {

        if (Input.GetKey(KeyCode.LeftControl))
        {
            state = States.crouching;
        }
        else base.StateHandler();
    }
    public void FixedUpdate()
    {
        Crouching();
        base.FixedUpdate();
    }
    private void Update()
    {
        Animations();
        base.Update();
    }

    void Animations()
    {
        if (mAnimator)
        {
            mAnimator.SetBool("Slash", Input.GetKeyDown(KeyCode.Q));
        }
    }
    private void Crouching()
    {
        if (state == States.crouching)
        {
            if (!crouching)
            {
                GetComponent<CameraMovement>().cameraHeight = 0.6f;
                crouching = true;
                if (mAnimator) mAnimator.SetBool("Crouch", crouching);
                caps1.enabled = false;
                caps2.enabled = true;
                
                speed = crouchSpeed;
            }
        }
        else if (crouching)
        {
            GetComponent<CameraMovement>().cameraHeight = 1.6f;
            
            //transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y / CrouchMultiplier, transform.localScale.z);
            crouching = false;
            caps1.enabled = true;
            caps2.enabled = false;
            if (mAnimator) mAnimator.SetBool("Crouch", crouching);
            speed = oldSpeed;
        }
    }

}
