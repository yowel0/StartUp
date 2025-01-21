using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    bool endGame = false;
    private void Awake()
    {

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

    private void Update()
    {
        CheckClientsTaskRpc();
    }
    public void EndGame(bool PoliceWon)
    {
        endGame = true;
        Debug.Log("game end");
    }

    [Rpc(SendTo.ClientsAndHost)]
    public void CheckClientsTaskRpc()
    {
        print("testsakibhiidi");
    }
}
