using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private string scenename;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.anyKey)
        {
            Debug.Log("Key was released, loading " + scenename);

            SceneManager.LoadScene(scenename);
        }
    }
}
