using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkManagerButtons : MonoBehaviour
{
    [SerializeField]
    NetworkManager networkManager;

    
    public void StartHost(){
        networkManager.StartHost();
    }

    public void StartClient(){
        networkManager.StartClient();
    }

    public void StartServer(){
        networkManager.StartServer();
    }
}
