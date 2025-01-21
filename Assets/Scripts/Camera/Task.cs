using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Task : MonoBehaviour
{

    [SerializeField]
    EvidenceCheck evidenceCheck;

    [SerializeField]
    GameObject taskEvidence;

    [SerializeField]
    TextMeshProUGUI text;

    [SerializeField]
    GameObject checkMark;

    bool displayed = false;

    private int index = 0;

    [SerializeField]
    private Image photoDisplayArea;

    private void Start()
    {
    }
    void Update()
    {
            check();
        
    }

    void check()
    {
        if (taskEvidence != null)
        {
            if (evidenceCheck.foundEvidence.Contains(taskEvidence) && !displayed)
            {
                evidenceCheck.ShowPhoto();
                photoDisplayArea.sprite = evidenceCheck.takenPhotos[evidenceCheck.takenPhotos.Count - 1];
                checkMark.SetActive(true);
                text.text = taskEvidence.tag;
                displayed = true;
            }
        }
    }
}
