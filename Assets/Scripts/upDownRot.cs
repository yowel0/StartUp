using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class upDownRot : MonoBehaviour
{
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = transform.parent.GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cam != null)
        {
            Debug.Log("got a cam");
            this.transform.rotation = Quaternion.Euler(cam.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z);
        }
    }
}
