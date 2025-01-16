using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefBehaviour : MoveBehaviour
{
    [SerializeField] float crouchMult;
    [SerializeField] float crouchDrag;

    AudioSource audio;

    public bool grabbed = false;
    public int shakes = 1;

    [SerializeField] List<Transform> copsInRadius;
    Transform closestCop;
    bool policeClose = false;
    [SerializeField] float distClosestCop = 100;

    bool crouching = false;
    bool shakeLeft = true;
    bool isPLaying = false;
    float oldStopDrag;

    public void Start()
    {
        base.Start();
        audio = GetComponent<AudioSource>();
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
        HeartBeat();
        EscapeGrab();
        base.Update();
    }

    void HeartBeat()
    {
        if (policeClose)
        {
            for (int i = 0; i < copsInRadius.Count; i++)
            {
                float dist = (copsInRadius[i].position - transform.position).magnitude;
                if (dist < distClosestCop || closestCop == null)
                {
                    closestCop = copsInRadius[i];
                }
            }
            distClosestCop = (closestCop.position - transform.position).magnitude;
            audio.pitch = 2 - (distClosestCop/10);
            
            //Add a script to change the speed/volume of the heartbeat audio based on the distClosestCop.
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PoliceBehaviour>() != null)
            {
                policeClose = true;
                copsInRadius.Add(other.transform);
                if (!isPLaying)
                {
                    audio.Play();
                    isPLaying = true;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PoliceBehaviour>() != null)
            {
                if (other.transform == closestCop)
                    closestCop = null;
                
                copsInRadius.Remove(other.transform);
                if (copsInRadius.Count == 0)
                {
                    audio.Stop();
                    isPLaying = false;
                    policeClose = false;
                }
            }
        }
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
