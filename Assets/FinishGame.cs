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
    [SerializeField]
    InteractionsMurderer murderer;
    public enum ExitCheckType
    {
        FoundTasks,
        CleanedUpTasks
    }
    public ExitCheckType exitCheckType;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (murderer.CleanupAmount >= foundEvidenceAmount)
        {
            print("sigma");
        }
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
        switch (exitCheckType)
        {
            case ExitCheckType.FoundTasks:
                if (TaskManager.instance.FoundTasksAmount() >= foundEvidenceAmount)
                {
                    return true;
                }
                break;
            case ExitCheckType.CleanedUpTasks:
                if (murderer.CleanupAmount >= foundEvidenceAmount)
                {
                    return true;
                }
                break;

        }
        return false;
    }
}
