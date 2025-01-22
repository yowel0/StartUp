using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioTestScript : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("C key was pressed");
            AudioManager.instance.PlayOneShot(FMODEvents.instance.takePhotograph, this.transform.position);
        }
    }
}
