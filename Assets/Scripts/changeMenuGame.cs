using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeMenuGame : MonoBehaviour
{
    [SerializeField]
    GameObject oldMenu;
    [SerializeField]
    GameObject controlMenu;
    [SerializeField]
    GameObject audioMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void NewMenu(GameObject newMenu)
    {
        
        newMenu.SetActive(true);
    }
    public void OldMenu()
    {
        oldMenu.SetActive(false);
    }
    public void closeNewMenu(GameObject newwMenu)
    {
        newwMenu.SetActive(false);
    }
    public void openOldMenu(GameObject olddMenu)
    {
        olddMenu.SetActive(true);
    }
    
}
