using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class Task : MonoBehaviour
{

    [SerializeField]
    EvidenceCheck evidenceCheck;

    [SerializeField]
    GameObject[] checkMarks;

    [SerializeField]
    GameObject[] photoDisplayArea;

    int index = 0;

    [SerializeField]
    private List<GameObject> oldFoundEvidence;

    private void Start()
    {
        evidenceCheck = FindObjectOfType<EvidenceCheck>();
        checkMarks = GameObject.FindGameObjectsWithTag("Checkmark");
        photoDisplayArea = GameObject.FindGameObjectsWithTag("PhotoDisplay");
        oldFoundEvidence = evidenceCheck.foundEvidence;
    }

    void Update()
    {
        if (evidenceCheck == null)
        {
            evidenceCheck = FindObjectOfType<EvidenceCheck>();
        }
        else
        {
            if (oldFoundEvidence != evidenceCheck.foundEvidence)
            {
                //print("isigma");
                check();
                oldFoundEvidence = evidenceCheck.foundEvidence;
            }
        }
    }

    void check()
    {
        if(evidenceCheck != null) { evidenceCheck.ShowPhoto(); }
        photoDisplayArea[index].GetComponent<Image>().sprite = evidenceCheck.takenPhotos[evidenceCheck.takenPhotos.Count-1];
        checkMarks[index].SetActive(true);
        index++; 
    }
}
