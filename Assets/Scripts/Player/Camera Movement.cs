using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private float sensX = 100;
    [SerializeField]
    private float sensY = 100;
    bool check = false;

    [SerializeField]
    float cameraHeight = 1.5f;

    SpotLight light;

    float xRotation;
    [SerializeField] float yRotation;

    Camera camera;

    /*public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner)
        {

            this.enabled = false;
        }
        
    }*/

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        camera = Camera.main;
        if (!this.GetComponent<ThiefBehaviour>())
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource)
            {
                audioSource.enabled = false;
            }
        }

    }

    private void Update()
    {
        if (!check && this.GetComponent<PoliceBehaviour>() != null)
        {
            ThiefBehaviour thief = FindFirstObjectByType<ThiefBehaviour>();
            if (thief != null && !thief.GetComponent<NetworkObject>().IsOwner){
                thief.GetComponentInChildren<Light>(true).enabled = false;
                check = true;
            }
        }
        camera.transform.position = transform.position + new Vector3(0, cameraHeight);

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        yRotation = yRotation % 360;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
/*        print(xRotation + " " + yRotation);*/
 
        camera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
