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
    private bool everythingFound;
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
            if (task.evidenceObject.CompareTag(_evidence.tag))
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
        public string taskName = "skibidy";
        public Image displayArea;
        public GameObject evidenceObject;
        public GameObject checkMark;
        public GameObject GetObjectInGame()
        {
            return GameObject.FindGameObjectWithTag(evidenceObject.tag);
        }
        public void Find(Sprite _sprite)
        {
            if (!found)
            {
                print(" finded");
                found = true;
                displayArea.sprite = _sprite;
                displayArea.rectTransform.localScale = new Vector3(3, 1, 1);
                checkMark.SetActive(true);
            }
        }
    }
}
