using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }
    bool endGame = false;
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if(!IsServer)
        {
            Destroy(gameObject);
            return;
        }
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
    public void EndGame(bool PoliceWon)
    {
        endGame = true;
        Debug.Log("game end");
    }
    struct ForceNetworkSerializeByMemcpy<GameObject>
    {

    }

    [Rpc(SendTo.ClientsAndHost)]
    public void CheckClientsTaskRpc(int id)
    {
        GameObject ob = GameObject.Find(id.ToString());
        EvidenceCheck ev = ob.GetComponent<EvidenceCheck>();
        EvidenceCheck[] evidence = FindObjectsOfType<EvidenceCheck>();
        for (int i = 0; i < evidence.Length; i++)
        {
            evidence[i].evidence = ev.evidence;
            evidence[i].foundEvidence = ev.foundEvidence;
        }
    }
}
