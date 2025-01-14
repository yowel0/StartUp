using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotoCapture : MonoBehaviour
{

    private Texture2D screenCapture;

    [SerializeField]
    private GameObject Phone;

    

    bool phoneIsOut;

    private void Start()
    {
        screenCapture = new Texture2D(Screen.width, Screen.height);
    }
    void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.F))
        {
            if (phoneIsOut)
            {
                phoneIsOut = false;
            }
            else
            {
                phoneIsOut = true;
            }
            
        }

        if (phoneIsOut)
        {
            Phone.SetActive(true);
            
        }
        else
        {
            Phone.SetActive(false);
        }

        if( phoneIsOut && Input.GetMouseButton(0))
        {
            
        }
    }

    /*IEnumerator CapturePhoto()
    {
        yield return new WaitForEndOfFrame();

        Rect regionToRead = new Rect(0,0, Screen.width, Screen.height);

        screenCapture.ReadPixels(regionToRead, 0, 0, false);
        screenCapture.Apply();
    }*/
}
