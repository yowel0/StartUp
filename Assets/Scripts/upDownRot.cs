using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class upDownRot : NetworkBehaviour
{
    Camera cam;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        
        if (!IsOwner){
            enabled = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (cam != null)
        {
            transform.position = cam.transform.position;
            transform.rotation = cam.transform.rotation;
        }
    }
}
