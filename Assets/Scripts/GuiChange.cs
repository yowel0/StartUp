using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiChange : MonoBehaviour
{

    int curScreen = 0;
    List<GameObject> screens = new List<GameObject>();
    bool nextScreen = false;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            screens.Add(transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            if (i != curScreen)
            {
                screens[i].gameObject.SetActive(false);
            }
            else screens[i].gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ScreenSwitch();
       
    }

    public void NextScreen(int screen)
    {
        nextScreen = true;
        curScreen = screen - 1;
    }
    void ScreenSwitch()
    {

        if (nextScreen && curScreen < transform.childCount)
        {
            screens[curScreen - 1].gameObject.SetActive(false);
            screens[curScreen].gameObject.SetActive(true);
            nextScreen = false;
        }
    }
   
}
