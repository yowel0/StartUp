using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpening : MonoBehaviour
{
    [SerializeField] LayerMask doorLayer;
    Camera cam;
    Transform hinge;
    Transform door;
    float maxRot = 90;
    [SerializeField] float rot = 0;
    bool gotDoor = false;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 10, doorLayer))
        {
            if (Input.GetMouseButtonDown(0))
            {
                door = hit.transform;
                hinge = hit.transform.parent.transform;
                rot = hinge.transform.localRotation.y * 120;
                print(rot);
                gotDoor = true;

            }
        }
        if (gotDoor)
        {
            door.GetComponent<BoxCollider>().enabled = false;
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * 400;
            rot -= mouseX;
            rot = Mathf.Clamp(rot, 0, 90f);
            hinge.rotation = Quaternion.Euler(0, hinge.rotation.y + rot, 0);
            if (hinge.GetComponentInChildren<DoorController>().firstTime)
            {
                hinge.rotation = Quaternion.Euler(0, hinge.rotation.y + 90, 0);
                hinge.GetComponentInChildren<DoorController>().firstTime = false;
            }
        }
        if (Input.GetMouseButtonUp(0) && hinge)
        {
            door.GetComponent<BoxCollider>().enabled = true;
            hinge = null;
            door = null;
            gotDoor = false;
            rot = 0;
        }
    }
}
