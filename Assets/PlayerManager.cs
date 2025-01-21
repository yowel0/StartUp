using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Netcode;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    public static PlayerManager instance { get; private set; }

    public List<Player> playerList = new List<Player>();
    bool eventsAdded = false;

    [SerializeField]
    GameObject spherePrefab;
    void Awake(){
        if (instance == null){
            instance = this;
        }
        else{
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        if (!NetworkManager.Singleton.IsServer){
            return;
        }
        base.OnNetworkSpawn();
        DontDestroyOnLoad(this);
        NetworkManager.Singleton.OnClientConnectedCallback += AddPlayer;
        //NetworkManager.Singleton.OnClientDisconnectCallback += RemovePlayer;
    }

    public override void OnNetworkDespawn()
    {
        if (!NetworkManager.Singleton.IsServer){
            return;
        }
        base.OnNetworkDespawn();
        NetworkManager.Singleton.OnClientConnectedCallback -= AddPlayer;
        //NetworkManager.Singleton.OnClientDisconnectCallback -= RemovePlayer;
    }

    // private void OnEnable(){
    // NetworkManager.Singleton.OnClientConnectedCallback += AddPlayer;
    // NetworkManager.Singleton.OnClientDisconnectCallback += RemovePlayer;
    // }

    // private void OnDisable(){
    //     NetworkManager.Singleton.OnClientConnectedCallback -= AddPlayer;
    //     NetworkManager.Singleton.OnClientDisconnectCallback -= RemovePlayer;
    // }

    void AddPlayer(ulong _ulong){
        NetworkManager.Singleton.ConnectedClients.TryGetValue(_ulong, out var client);
        playerList.Add(client.PlayerObject.GetComponent<Player>());
    }

    void RemovePlayer(ulong _ulong){
        NetworkManager.Singleton.ConnectedClients.TryGetValue(_ulong, out var client);
        playerList.Remove(client.PlayerObject.GetComponent<Player>());
    }

    void ReloadPlayerList(){
        playerList = new List<Player>();
        foreach (KeyValuePair<ulong,NetworkClient> client in NetworkManager.Singleton.ConnectedClients) {
            playerList.Add(client.Value.PlayerObject.GetComponent<Player>());
        }
    }

    public void spawnSphere(ulong _OwnerClientId,NetworkObject _networkObjectPrefab){
        print("Im a playermanager request from: " + _OwnerClientId);
        //var instance = Instantiate(spherePrefab);
        //instance.transform.position = new Vector3(_OwnerClientId, 0, 0);
        //var instanceNetworkObject = instance.GetComponent<NetworkObject>();
        //instanceNetworkObject.SpawnWithOwnership(_OwnerClientId);
        NetworkManager.SpawnManager.InstantiateAndSpawn(_networkObjectPrefab,_OwnerClientId,false,true,false,new Vector3(_OwnerClientId,0,0));
    }
}
