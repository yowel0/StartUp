using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class ThiefBehaviour : MoveBehaviour
{

    [SerializeField] float crouchSpeed;
    [SerializeField] Transform holdPos;

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

    [SerializeField] float minCleanTime;
    [SerializeField] float cleanTimer = 0;
    bool cleaning = false;
    Slider slider;
    Image[] sliderParts;

    GameObject knife;
    bool holdingKnife = false;


    [SerializeField]
    float CrouchMultiplier = 0.5f;
    public void Start()
    {
        cam = Camera.main;
        slider = cam.GetComponentInChildren<Slider>(true);
        sliderParts = slider.GetComponentsInChildren<Image>(true);
        foreach (Image part in sliderParts)
            part.enabled = false;
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
            if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, 3) && !hiding && !grabbed)
            {
                obj = hit.transform;
                if (obj.CompareTag("Closet"))
                {
                    Hiding();
                }
                else if (obj.gameObject.layer == LayerMask.NameToLayer("Evidence") && !cleaning)
                {
                    foreach (Image part in sliderParts)
                        part.enabled = true;
                    Debug.Log("check");
                    cleaning = true;
                }
                else if (obj.gameObject.CompareTag("Knife"))
                {
                    print("grab");
                    knife = obj.gameObject;
                    KnifePickUp(true, false);
                }
                else if (holdingKnife)
                {
                    print("holding");
                    KnifePickUp(false, false);
                }
            }
            else if (holdingKnife)
            {
                print("holding");
                KnifePickUp(false, false);
            }
            else
            {
                if (hiding)
                {
                    Hiding();
                }
            }
        }
        if (holdingKnife)
        {
            KnifePickUp(false, true);
        }
        StopCleaning();
        HideAnim();
        HeartBeat();
        EscapeGrab();
        base.Update();
    }

    void KnifePickUp(bool grabbing, bool holding)
    {
        if (grabbing && !holding)
        {
            knife.gameObject.GetComponent<Collider>().isTrigger = true;
            knife.gameObject.GetComponent<Rigidbody>().useGravity = false;
            knife.transform.position = holdPos.position;
            knife.transform.rotation = holdPos.rotation;
            knife.transform.SetParent(holdPos);
            
            knife.GetComponent<KnifeScript>().enabled = true;
            knife.GetComponent<KnifeScript>().grabbed = true;
            holdingKnife = true;
        }
        else if (!grabbing && !holding)
        {
            knife.gameObject.GetComponent<Collider>().isTrigger = false;
            knife.gameObject.GetComponent<Rigidbody>().useGravity = true;
            knife.GetComponent<KnifeScript>().enabled = false;
            knife.GetComponent<KnifeScript>().grabbed = false;
            knife.transform.SetParent(null);
            holdingKnife = false;
        }
        else if (!grabbing && holding)
        {
            knife.transform.position = holdPos.position;
            knife.transform.rotation = holdPos.rotation;
        }
    }

    void StopCleaning()
    {
        if (cleaning)
        {
            RaycastHit looking;
            cleanTimer += Time.deltaTime;
            if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out looking, 50) && !hiding && !grabbed)
            {
                if (looking.transform.gameObject.layer != LayerMask.NameToLayer("Evidence") || !Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), 50))
                {
                    Debug.Log("check2");
                    cleaning = false;
                    cleanTimer = 0;
                    obj = null;
                    foreach (Image part in sliderParts)
                        part.enabled = false;
                }
                if (cleanTimer > minCleanTime)
                {
                    slider.enabled = false;
                    Destroy(looking.transform.gameObject);
                    //Evidence Destroyed + 1;
                    obj = null;
                    cleaning = false;
                    cleanTimer = 0;
                    foreach (Image part in sliderParts)
                        part.enabled = false;
                }
            }
            slider.value = cleanTimer / minCleanTime;
        }
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
            audio.volume = 2 - (distClosestCop / 10);

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
