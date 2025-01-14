using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefBehaviour : MoveBehaviour
{
    [SerializeField] float crouchMult;
    [SerializeField] float crouchDrag;
    public bool grabbed = false;
    bool crouching = false;
    bool shakeLeft = true;
    public int shakes = 1;
    float oldStopDrag;

    public void Start()
    {
        base.Start();
        oldStopDrag = stopDrag;
    }
    public override void StateHandler()
    {

        if (Input.GetKey(KeyCode.LeftControl))
        {
            state = States.crouching;
            speedMult = crouchMult;
        }
        else base.StateHandler();
    }
    public void FixedUpdate()
    {
        Crouching();
        base.FixedUpdate();
    }
    public void Update()
    {
        EscapeGrab();
        base.Update();
    }

    public override void Stopping()
    {
        if (state == States.crouching)
        {

        }
        else
        {
            base.Stopping();
        }
    }

    private void Crouching()
    {
        if (state == States.crouching)
        {
            if (!crouching)
            {
                crouching = true;
                transform.localScale = new Vector3(1, 0.7f, 1);
                transform.position = new Vector3(transform.position.x, transform.position.y - 0.3f, transform.position.z);
            }
            drag = crouchDrag;
        }
        else if(crouching)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y+0.3f, transform.position.z);
            transform.localScale = new Vector3(1, 1, 1);
            crouching=false;
        }
    }
    private void EscapeGrab()
    {
        if (grabbed)
        {
            
            if(Input.GetKeyDown(KeyCode.J) && shakeLeft)
            {
                shakes -= 1;
                shakeLeft = false;
            }
            else if(Input.GetKeyDown(KeyCode.L) && !shakeLeft)
            {
                shakes -= 1;
                shakeLeft = true;
            }
        }
    }
}
