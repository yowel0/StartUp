using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class InteractionsMurderer : MonoBehaviour
{
    [SerializeField] Transform holdPos;

    [SerializeField] Camera cam;

    public bool grabbed = false;
    public int shakes = 1;

    bool shakeLeft = true;


    public bool hiding = false;
    bool canExit = false;
    float elapsedTime;
    [SerializeField] float desiredDur = 2;
    Transform obj;

    [SerializeField] float minCleanTime;
    [SerializeField] float cleanTimer = 0;
    bool cleaning = false;
    Slider slider;
    Image[] sliderParts;

    GameObject knife;
    bool holdingKnife = false;

    Animator animator;
    Rigidbody rb;
    MoveBehaviour moveBehaviour;
    private void Start()
    {
        cam = Camera.main;
        slider = cam.GetComponentInChildren<Slider>(true);
        sliderParts = slider.GetComponentsInChildren<Image>(true);
        foreach (Image part in sliderParts)
        {
            part.enabled = false;
        }
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        moveBehaviour = GetComponent<MoveBehaviour>();
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
                    animator.SetBool("Removing ev", true);
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
        EscapeGrab();

        if (grabbed)
        {
            rb.isKinematic = false;
        }
    }

    //Knife Mech
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

    //CleaningMech
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
                    animator.SetBool("Removing ev", false);
                    Debug.Log("check2");
                    cleaning = false;
                    cleanTimer = 0;
                    obj = null;
                    foreach (Image part in sliderParts)
                        part.enabled = false;
                }
                if (cleanTimer > minCleanTime)
                {
                    animator.SetBool("Removing ev", false);
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

    //Hiding Mech
    void Hiding()
    {
        // Does the ray intersect any objects excluding the player layer

        if (!hiding && !canExit)
        {
            this.GetComponent<CapsuleCollider>().enabled = false;
            rb.useGravity = false;
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
            transform.position = Vector3.Lerp(transform.position, obj.GetChild(1).transform.position, Mathf.SmoothStep(0, 1, perc));
            moveBehaviour.grounded = true;
            moveBehaviour.checkForGround = false;
            if (perc >= 0.5f)
                canExit = true;
        }
        if (!hiding && canExit)
        {
            elapsedTime += Time.deltaTime;
            float perc = elapsedTime / desiredDur;
            transform.position = Vector3.Lerp(transform.position, obj.GetChild(0).transform.position, Mathf.SmoothStep(0, 1, perc));
            moveBehaviour.checkForGround = false;
            moveBehaviour.grounded = true;
            if (perc >= 0.5f)
            {
                moveBehaviour.checkForGround = true;
                this.GetComponent<CapsuleCollider>().enabled = true;
                rb.useGravity = true;
                canExit = false;
                obj = null;
                elapsedTime = 0;
            }
        }
    }

    //Grabbed Mech
    private void EscapeGrab()
    {
        if (grabbed)
        {

            if (Input.GetKeyDown(KeyCode.A) && shakeLeft)
            {
                shakes -= 1;
                shakeLeft = false;
                animator.SetBool("Captured", true);
            }
            else if (Input.GetKeyDown(KeyCode.D) && !shakeLeft)
            {
                shakes -= 1;
                shakeLeft = true;
                animator.SetBool("Captured", true);
            }
            animator.SetBool("Captured", false);
        }
    }
}
