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
    [SerializeField] int minShakes;
    [SerializeField] int maxShakes;
    ThiefBehaviour script;
    GameObject obj = null;
    bool grabbed = false;
    bool canGrab = true;
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
            if (grabbed && obj != null)
            {
                LetGo();
            }
            if (Physics.Raycast(cam.transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, 50) && canGrab && !grabbed)
            {
                obj = hit.transform.gameObject;
                Grabbing();
            }
        }
        if (grabbed)
        {
            obj.transform.position = holdPos.position;
            obj.transform.rotation = holdPos.rotation;
            if(script.shakes <= 0)
            {
                LetGo();
            }
        }
        canGrab = true;
        base.Update();

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
        Rigidbody rig = obj.GetComponent<Rigidbody>();
        rig.useGravity = true;
        CapsuleCollider caps = obj.GetComponent<CapsuleCollider>();
        caps.enabled = true;
        obj = null;
        canGrab = false;
        grabbed = false;
    }
}
