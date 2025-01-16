using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;

public class ThiefBehaviour : MoveBehaviour
{

    [SerializeField] float crouchMult;
    [SerializeField] float crouchDrag;

    AudioSource audio;
    [SerializeField] Camera cam;

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

    bool hiding = false;
    bool canExit =false;
    float elapsedTime;
    [SerializeField] float check;
    [SerializeField] float desiredDur = 2;
    Transform obj;

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
        Hiding();
        HeartBeat();
        EscapeGrab();
        base.Update();
    }

    void Hiding()
    {
        // Does the ray intersect any objects excluding the player layer

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!hiding && !canExit)
            {
                RaycastHit hit;
                if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, 50) && !hiding && !grabbed)
                {
                    if (hit.transform.parent.CompareTag("Closet"))
                    {
                        this.GetComponent<CapsuleCollider>().enabled = false;
                        base.rb.useGravity = false;
                        obj = hit.transform.parent;
                        transform.position = obj.GetChild(0).transform.position;
                        transform.LookAt(obj.GetChild(1).transform.position);
                        hiding = true;
                        canExit = false;
                    }
                }
            }
            if (hiding && canExit == true)
            {
                hiding = false;
                elapsedTime = 0;
            }
        }
        if (hiding)
        {
            elapsedTime += Time.deltaTime;
            float perc = elapsedTime / desiredDur;
            check = perc;
            transform.position = Vector3.Lerp(transform.position, obj.GetChild(1).transform.position, Mathf.SmoothStep(0,1,perc));
            if (perc >= 0.5f)
            canExit=true;
        }
        if(!hiding && canExit)
        {
            elapsedTime += Time.deltaTime;
            float perc = elapsedTime / desiredDur;
            check = perc;
            transform.position = Vector3.Lerp(transform.position, obj.GetChild(0).transform.position, Mathf.SmoothStep(0, 1, perc));
            if (perc >= 0.5f)
            {
                this.GetComponent<CapsuleCollider>().enabled = true;
                rb.useGravity = true;
                canExit = false;
                obj = null;
                elapsedTime = 0;
            }
        }
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
