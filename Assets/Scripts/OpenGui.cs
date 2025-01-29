using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenGui : MonoBehaviour
{
    [SerializeField] GameObject guiPopUp;
  

   
    // Start is called before the first frame update
    void Awake()
    {

     guiPopUp.SetActive(false);
     
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape)&& guiPopUp.activeInHierarchy == false) 
        { 
        openGui();

        
        } 
        else if (Input.GetKeyUp(KeyCode.Escape)&& guiPopUp.activeInHierarchy == true)
        {
            closeGui();
        }
        
    }

    public void openGui()
    {
        guiPopUp.SetActive(true);
        Debug.Log("GUI open");
    }
    public void closeGui()
    {
        guiPopUp.SetActive(false);
        Debug.Log("GUI closed");
    }

  
}
