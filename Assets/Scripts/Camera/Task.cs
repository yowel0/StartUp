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
    Material checkMark;

    [SerializeField]
    private Image photoDisplayArea;

    UnityEvent checkIfPhotoTaken;

    private List<GameObject> oldFoundEvidence;

    private void Start()
    {
        oldFoundEvidence = evidenceCheck.foundEvidence;
        evidenceCheck = FindObjectOfType<EvidenceCheck>();
        checkIfPhotoTaken.AddListener(check);
    }

    void Update()
    {
        if(oldFoundEvidence != evidenceCheck.foundEvidence)
        {
            checkIfPhotoTaken.Invoke();
            oldFoundEvidence = evidenceCheck.foundEvidence;
        }
    }

    void check()
    {
            if(evidenceCheck != null) { evidenceCheck.ShowPhoto(); }
            photoDisplayArea.sprite = evidenceCheck.takenPhotos[evidenceCheck.takenPhotos.Count-1];
            checkMark.color = Color.green;
    }
}
