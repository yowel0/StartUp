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
    List<GameObject> checkMarks = new List<GameObject>();

    [SerializeField]
    private Image[] photoDisplayArea = new Image[10];

    int index = 0;

    [SerializeField]
    private List<GameObject> oldFoundEvidence;

    private void Start()
    {
        evidenceCheck = GetComponentInParent<EvidenceCheck>();
        photoDisplayArea = GetComponentsInChildren<Image>();
        oldFoundEvidence = evidenceCheck.foundEvidence;
        foreach (GameObject child in children)
        {
            if (child.CompareTag("Checkmark"))
            {
                checkMarks.Add(child);
            }
        }
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
                check();
                oldFoundEvidence = evidenceCheck.foundEvidence;
            }
        }
    }

    void check()
    {
        
        if(evidenceCheck != null) { evidenceCheck.ShowPhoto(); }
        photoDisplayArea[index].sprite = evidenceCheck.takenPhotos[evidenceCheck.takenPhotos.Count-1];
        checkMarks[index].SetActive(true);
        index++;
            
    }
}
