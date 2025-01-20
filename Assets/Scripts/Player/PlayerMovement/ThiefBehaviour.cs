using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;

public class ThiefBehaviour : MoveBehaviour
{

    [SerializeField] float crouchSpeed;

    AudioSource audio;
    [SerializeField] Camera cam;

    public bool grabbed = false;
    public int shakes = 1;

    List<Transform> copsInRadius = new List<Transform>();
    Transform closestCop;
    bool policeClose = false;
    [SerializeField] float distClosestCop = 100;

    bool crouching = false;
    bool shakeLeft = true;
    bool isPLaying = false;
    float oldSpeed;

    bool hiding = false;
    bool canExit = false;
    float elapsedTime;
    [SerializeField] float check;
    [SerializeField] float desiredDur = 2;
    Transform obj;

    public void Start()
    {
        base.Start();
        audio = GetComponent<AudioSource>();
        oldSpeed = speed;
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
    public void Update()
    {
        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, 50) && !hiding && !grabbed)
            {
                obj = hit.transform.parent;
                if (hit.transform.parent.CompareTag("Closet"))
                {
                    Hiding();
                }

            }
            else
            {
                Hiding();
            }
        }
        HideAnim();
        HeartBeat();
        EscapeGrab();
        base.Update();
    }

    void Hiding()
    {
        // Does the ray intersect any objects excluding the player layer

        if (!hiding && !canExit)
        {
            this.GetComponent<CapsuleCollider>().enabled = false;
            base.rb.useGravity = false;
            transform.position = obj.GetChild(0).transform.position;
            transform.LookAt(obj.GetChild(1).transform.position);
            hiding = true;
            canExit = false;
        }
        if (hiding && canExit == true)
        {
            hiding = false;
            elapsedTime = 0;
        }
    }

    void HideAnim()
    {
        if (hiding)
        {
            elapsedTime += Time.deltaTime;
            float perc = elapsedTime / desiredDur;
            check = perc;
            transform.position = Vector3.Lerp(transform.position, obj.GetChild(1).transform.position, Mathf.SmoothStep(0, 1, perc));
            base.grounded = true;
            base.checkForGround = false;
            if (perc >= 0.5f)
                canExit = true;
        }
        if (!hiding && canExit)
        {
            elapsedTime += Time.deltaTime;
            float perc = elapsedTime / desiredDur;
            check = perc;
            transform.position = Vector3.Lerp(transform.position, obj.GetChild(0).transform.position, Mathf.SmoothStep(0, 1, perc));
            base.checkForGround = false;
            base.grounded = true;
            if (perc >= 0.5f)
            {
                base.checkForGround = true;
                this.GetComponent<CapsuleCollider>().enabled = true;
                rb.useGravity = true;
                canExit = false;
                obj = null;
                elapsedTime = 0;
            }
        }
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
            audio.pitch = 2 - (distClosestCop / 10);

            //Add a script to change the speed/volume of the heartbeat audio based on the distClosestCop.
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.GetComponent<PoliceBehaviour>())
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

    private void Crouching()
    {
        if (state == States.crouching)
        {
            if (!crouching)
            {
                crouching = true;
                transform.localScale = new Vector3(1, 0.7f, 1);
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
    private void EscapeGrab()
    {
        if (grabbed)
        {

            if (Input.GetKeyDown(KeyCode.J) && shakeLeft)
            {
                shakes -= 1;
                shakeLeft = false;
            }
            else if (Input.GetKeyDown(KeyCode.L) && !shakeLeft)
            {
                shakes -= 1;
                shakeLeft = true;
            }
        }
    }
}
