using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    [SerializeField] string nextScene;
    bool onTime = true;
    float goalTime = 5;
    float timer;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (onTime)
        {
            timer += Time.deltaTime;
            if (timer >= goalTime)
            {
                SceneManager.LoadScene(nextScene);
            }
        }
    }

    public void OnClick()
    {
        SceneManager.LoadScene(nextScene);
    }
}
