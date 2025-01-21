using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CameraFollowScript : NetworkBehaviour
{
    Camera camera;
    [SerializeField]
    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner){
            this.enabled = false;
        }
        camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        camera.transform.position = transform.position + offset;
    }
}
