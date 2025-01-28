using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
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
    public void NextScreen()
    {
        nextScreen = true;
    }
    void ScreenSwitch()
    {
        if ((Input.anyKeyDown) && curScreen == 0)
        {
            nextScreen = true;
        }
        if (nextScreen)
        {
            curScreen += 1;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (i != curScreen)
                {
                    screens[i].gameObject.SetActive(false);
                }
                else screens[i].gameObject.SetActive(true);
            }
            nextScreen = false;
        }
    }
}
