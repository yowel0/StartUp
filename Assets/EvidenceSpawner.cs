using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class EvidenceSpawner : MonoBehaviour
{
    public List<Evidence> m_EvidenceList;
    public GameObject[] windows;
    public GameObject brokenWindow;
    void Start()
    {
        foreach (Evidence evidence in m_EvidenceList)
        {
            evidence.SpawnRandom();
        }
        SpawnWindow();
    }
    public void SpawnWindow()
    {
        int spawnIndex = UnityEngine.Random.Range(0, windows.Length);
        brokenWindow = Instantiate(brokenWindow, windows[spawnIndex].transform);
        windows[spawnIndex].gameObject.SetActive(false);
    }


    [Serializable]
    public class Evidence
    {
        public string name = "evidence";
        public GameObject evidenceObj;

        public Transform[] spawnPositions;

        public void SpawnRandom()
        {
            int spawnIndex = UnityEngine.Random.Range(0, spawnPositions.Length);
            evidenceObj = Instantiate(evidenceObj, spawnPositions[spawnIndex]);
        }
        
    }
   
}
