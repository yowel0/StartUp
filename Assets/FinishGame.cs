using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishGame : MonoBehaviour
{
    [SerializeField]
    int foundEvidenceAmount;
    [SerializeField]
    string sceneToLoad;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("finished: " + CheckFinish());
        if (CheckFinish())
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    bool CheckFinish()
    {
        if (TaskManager.instance.FoundTasksAmount() >= foundEvidenceAmount)
        {
            return true;
        }
        return false;
    }
}
