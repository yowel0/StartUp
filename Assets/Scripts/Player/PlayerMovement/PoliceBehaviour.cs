using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceBehaviour : MoveBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Transform holdPos;
    [SerializeField] CapsuleCollider caps1;
    [SerializeField] CapsuleCollider caps2;
    [SerializeField] SphereCollider sphere;
    [SerializeField] int minShakes;
    [SerializeField] int maxShakes;
    public bool grabbed = false;
    ThiefBehaviour script;
    GameObject obj = null;
    bool canGrab = true;
    [SerializeField] int copsInProx = 0;
    float thiefSpeed;

    public void Start()
    {
        base.Start();
    }
    public void Update()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("check1");
            if (grabbed && obj != null)
            {
                LetGo();
            }
            if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, 50) && canGrab && !grabbed)
            {
                Debug.Log("check2");
                obj = hit.transform.gameObject;
                Grabbing();
            }
            canGrab = true;
        }
        if (grabbed)
        {
            Hold();
            EscapeCheck();
        }
        base.Update();

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PoliceBehaviour>() != null)
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
        if (script.shakes <= 0)
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
                script.shakes = UnityEngine.Random.Range(minShakes, maxShakes);
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
        if (obj.tag == "Murderer")
        {
            if (!grabbed)
            {
                script = obj.GetComponent<ThiefBehaviour>();
                thiefSpeed = script.speed;
                script.speed = 0;
                script.grabbed = true;
                script.shakes = UnityEngine.Random.Range(minShakes, maxShakes);
                caps1.enabled = false;
                caps2.enabled = true;
                sphere.enabled = true;
                
                obj.transform.position = holdPos.position;

                Rigidbody rig = obj.GetComponent<Rigidbody>();
                rig.useGravity = false;

                CapsuleCollider caps = obj.GetComponent<CapsuleCollider>();
                caps.enabled = false;




                grabbed = true;
            }
        }
    }
    private void LetGo()
    {
        caps1.enabled = true;
        caps2.enabled = false;
        sphere.enabled = false;
        script.speed = thiefSpeed;
        Rigidbody rig = obj.GetComponent<Rigidbody>();
        rig.useGravity = true;
        CapsuleCollider caps = obj.GetComponent<CapsuleCollider>();
        caps.enabled = true;
        script = null;
        obj = null;
        canGrab = false;
        grabbed = false;
    }
}
