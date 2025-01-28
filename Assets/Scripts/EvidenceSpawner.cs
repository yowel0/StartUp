using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceSpawner : MonoBehaviour
{
    public List<Evidence> m_EvidenceList;

    public GameObject[] Windows;
    void Start()
    {
        foreach (Evidence evidence in m_EvidenceList)
        {
            evidence.SpawnRandom();
        }
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
