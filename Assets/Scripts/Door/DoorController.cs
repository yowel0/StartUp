using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;

public class DoorPivot : MonoBehaviour
{
    [SerializeField] Camera cam;

    Transform selectedDoor;

    [SerializeField] GameObject dragPointGameObj;

    int leftDoor;

    [SerializeField] LayerMask doorLayer;
    // Update is called once per frame
    void Update()
    {
        
        RaycastHit hit;

        if(Physics.Raycast(cam.transform.position,cam.transform.forward, out hit, 1000, doorLayer))
        {
            if (Input.GetMouseButtonDown(0))
            {
                selectedDoor = hit.collider.gameObject.transform;
            }
        }

        if (selectedDoor != null)
        {
            
            HingeJoint joint = selectedDoor.GetComponent<HingeJoint>();
            JointMotor motor = joint.motor;
            if (dragPointGameObj == null)
            {   
                dragPointGameObj = new GameObject("Ray door");
                dragPointGameObj.transform.parent = selectedDoor;
            }

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            //dragPointGameObj.transform.position = ray.GetPoint(Vector3.Distance(selectedDoor.position, transform.position));
            dragPointGameObj.transform.rotation = selectedDoor.rotation;


            float delta = Mathf.Pow(Vector3.Distance(dragPointGameObj.transform.position, selectedDoor.position), 3);

            if (selectedDoor.GetComponent<MeshRenderer>().localBounds.center.x > selectedDoor.localPosition.x)
            {
                leftDoor = 1;
            }
            else
            {
                leftDoor = -1;
            }
          
            float speedMultiplier = 60000;
            if (Mathf.Abs(selectedDoor.forward.z) > 0.5f)
            {
                if (dragPointGameObj.transform.position.x > selectedDoor.position.x)
                {
                    motor.targetVelocity = delta * -speedMultiplier * Time.deltaTime * leftDoor;
                }
                else
                {
                    motor.targetVelocity = delta * speedMultiplier * Time.deltaTime * leftDoor;
                }
            }
            else
            {
                if (dragPointGameObj.transform.position.z > selectedDoor.position.z)
                {
                    motor.targetVelocity = delta * -speedMultiplier * Time.deltaTime * leftDoor;
                }
                else
                {
                    motor.targetVelocity = delta * speedMultiplier * Time.deltaTime * leftDoor;
                }
            }

            joint.motor = motor;
            if (Input.GetMouseButtonUp(0))
            {
                selectedDoor = null;
                motor.targetVelocity = 0;
                //Destroy(dragPointGameObj); 
            }
        }
    }
}
