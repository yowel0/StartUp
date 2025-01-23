using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskManager : MonoBehaviour
{
    public static TaskManager instance {  get; private set; }
    public List<Task> taskList = new List<Task>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    public bool CheckEvidence(GameObject _evidence)
    {
        foreach(Task task in taskList)
        {
            if (task.evidenceObject == _evidence)
            {
                return true;
            }
        }
        return false;
    }



    [Serializable]
    public class Task
    {
        public bool found;
        public string taskName;
        public Image displayArea;
        public GameObject evidenceObject;
        public GameObject checkMark;
        public void Find(Sprite _sprite)
        {
            if (!found)
            {
                found = true;
                displayArea.sprite = _sprite;
                checkMark.SetActive(true);
            }
        }
    }

    //void Update()
    //{
        //if (evidenceCheck == null)
        //{
        //    evidenceCheck = FindObjectOfType<EvidenceCheck>();
        //}
        //else
        //{
        //    if (oldFoundEvidence != evidenceCheck.foundEvidence)
        //    {
        //        print("isigma");
        //        check();
        //        oldFoundEvidence = evidenceCheck.foundEvidence;
        //    }
        //}
    //}

    //void check()
    //{
        //if (evidenceCheck != null) { evidenceCheck.ShowPhoto(); }
        //photoDisplayArea[index].GetComponent<Image>().sprite = evidenceCheck.takenPhotos[evidenceCheck.takenPhotos.Count - 1];
        //checkMarksArray[index].SetActive(true);
        //index++;
    //}
}
