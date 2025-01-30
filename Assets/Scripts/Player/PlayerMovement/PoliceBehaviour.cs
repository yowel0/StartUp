using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Networking.Transport;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PoliceBehaviour : MoveBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Transform holdPos;
    [SerializeField] CapsuleCollider caps1;
    [SerializeField] CapsuleCollider caps2;
    [SerializeField] SphereCollider sphere;
    [SerializeField] int minShakes;
    [SerializeField] int maxShakes;
    [SerializeField] float minPickUpTime;
    public bool grabbed = false;
    EvidenceCheck evidence;
    float pickUpTime = 0;
    ThiefBehaviour script;
    InteractionsMurderer script2;
    AIMurderer aiScript;
    GameObject obj = null;
    bool pickingUp = false;
    bool canGrab = true;
    [SerializeField] int copsInProx = 0;
    float thiefSpeed;


    public void Start()
    {

        base.Start();
        cam = Camera.main;
    }
    public void Update()
    {
        RayCastChecks();
        if (grabbed)
        {
            Hold();
            if (script2)
                EscapeCheck();
        }
        PickUp();
        base.Update();

    }
    void RayCastChecks()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("check1");
            if (grabbed && obj != null)
            {
                LetGo();
            }
            if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, 5) && canGrab && !grabbed)
            {
                Debug.Log("check2");
                obj = hit.transform.gameObject;
                if (hit.transform.CompareTag("Murderer"))
                {
                    Grabbing();
                }
            }
            canGrab = true;
        }
        else if (Input.GetKeyDown(KeyCode.E)){
            if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, 5) && canGrab && !grabbed)
            {
                obj = hit.transform.gameObject;
                if (hit.transform.gameObject.layer != LayerMask.NameToLayer("Evidence"))
                {
                    pickingUp = true;
                }
            }
        }
    }

    void PickUp()
    {
        if (pickingUp /*&& TaskManager.instance.CheckEvidence(obj)*/) //&& evidenceList.Contains(obj) )  
        {
            RaycastHit looking;
            pickUpTime += Time.deltaTime;
            if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out looking, 50) && !grabbed)
            {
                if (looking.transform.gameObject.layer != LayerMask.NameToLayer("Evidence") || !Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), 50))
                {
                    Debug.Log("check2");
                    pickingUp = false;
                    pickUpTime = 0;
                    obj = null;
                }
                if (pickUpTime > minPickUpTime)
                {
                    Destroy(looking.transform.gameObject);
                    /*DeleteObject(looking.transform.gameObject);*/
                    //Evidence Destroyed + 1;
                    obj = null;
                    pickingUp = false;
                    pickUpTime = 0;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PoliceBehaviour>() != null)
        {
            copsInProx++;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PoliceBehaviour>() != null)
        {
            copsInProx--;
        }
    }

    private void EscapeCheck()
    {
        if (script2.shakes <= 0)
        {
            int esc = UnityEngine.Random.Range(1, 11);
            if (copsInProx == 0 && esc >= 1 && esc <= 7)
            {
                LetGo();
            }
            else if (copsInProx == 1 && esc >= 1 && esc <= 3)
            {
                LetGo();
            }
            else
            {
                script2.shakes = UnityEngine.Random.Range(minShakes, maxShakes);
            }
        }
    }
    private void Hold()
    {
        obj.transform.position = holdPos.position;
        obj.transform.rotation = holdPos.rotation;
    }
    private void Grabbing()
    {
        if (!grabbed)
        {
            if (obj.GetComponent<ThiefBehaviour>() && obj.GetComponent<InteractionsMurderer>())
            {
                script = obj.GetComponent<ThiefBehaviour>();
                script2 = obj.GetComponent<InteractionsMurderer>();
                script2.shakes = UnityEngine.Random.Range(minShakes, maxShakes);
                thiefSpeed = script.speed;
                script.speed = 0;
                script.grabbed = true;
            }
            else
            {
                aiScript = obj.GetComponent<AIMurderer>();
                aiScript.grabbed = false;
            }
            caps1.enabled = false;
            caps2.enabled = true;
            sphere.enabled = true;

            obj.transform.position = holdPos.position;

            Rigidbody rig = obj.GetComponent<Rigidbody>();
            rig.useGravity = false;

            CapsuleCollider caps = obj.GetComponent<CapsuleCollider>();
            caps.enabled = false;

            obj.transform.SetParent(holdPos);

            grabbed = true;
        }
    }
    private void LetGo()
    {
        caps1.enabled = true;
        caps2.enabled = false;
        sphere.enabled = false;
        if (script)
        {
            script.speed = thiefSpeed;
            script.grabbed = false;
            script = null;
        }
        else
        {
            aiScript.grabbed = false;
            aiScript = null;
        }
        Rigidbody rig = obj.GetComponent<Rigidbody>();
        rig.useGravity = true;
        CapsuleCollider caps = obj.GetComponent<CapsuleCollider>();
        caps.enabled = true;
        obj.transform.SetParent(null);
        obj = null;
        canGrab = false;
        grabbed = false;
    }
}
