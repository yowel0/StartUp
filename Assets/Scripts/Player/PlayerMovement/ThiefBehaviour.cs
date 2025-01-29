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

    public bool grabbed = false;

    bool crouching = false;

    float oldSpeed;

    public void Start()
    {
        
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
            mAnimator.SetBool("Slash", Input.GetMouseButtonDown(0));
        }
    }
    private void Crouching()
    {
        if (state == States.crouching)
        {
            if (!crouching)
            {
                crouching = true;
                transform.localScale = new Vector3(1, CrouchMultiplier, 1);
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z);
                speed = crouchSpeed;
            }
        }
        else if (crouching)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);
            transform.localScale = new Vector3(1, 1, 1);
            crouching = false;
            speed = oldSpeed;
        }
    }
    
}
