using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PopUp : MonoBehaviour
{
    [SerializeField] GameObject popUp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPopUp()
    {
        popUp.SetActive(true);
    }
    public void ClosePopUp()
    {
        popUp.SetActive(false);
    }
}
