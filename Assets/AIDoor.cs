using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDoor : MonoBehaviour
{
    [SerializeField] Transform hinge;
    Quaternion rot;
    Transform door;
    bool openDoor;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Doors"))
        {
            print("RAHHHHH");
            hinge = other.transform.parent.transform;
            door = other.transform;
            rot = Quaternion.Euler(0, hinge.rotation.y + 80, 0);
            openDoor = true;
        }
    }
    private void Update()
    {
        if (openDoor)
        {
            hinge.rotation = rot;
            //door.rotation = rot;
        }
    }
}
