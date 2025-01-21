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
    Transform policeSpawnPos;
    [SerializeField]
    Transform KillerSpawnPos;
    
    public enum SpawnType{
        OneRandomKiller,
        OnlyPolice
    }

    [SerializeField]
    SpawnType spawnType;

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
        NetworkManager.Singleton.OnClientDisconnectCallback += RemovePlayer;
    }

    public override void OnNetworkDespawn()
    {
        if (!NetworkManager.Singleton.IsServer){
            return;
        }
        base.OnNetworkDespawn();
        NetworkManager.Singleton.OnClientConnectedCallback -= AddPlayer;
        NetworkManager.Singleton.OnClientDisconnectCallback -= RemovePlayer;
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

    public void StartRound(){
        switch (spawnType){
            case SpawnType.OneRandomKiller:
                SetRandomRoles();
                SpawnAllCharacters();
            return;
            case SpawnType.OnlyPolice:
                SetAllRoles(Player.Role.Police);
                SpawnAllCharacters();
            return;
        }
    }

    void SetRandomRoles(){
        int killerId = Random.Range(0,playerList.Count);
        for (int i = 0; i < playerList.Count; i++) {
            if (i == killerId){
                playerList[i].role = Player.Role.Killer;
            }
            else{
                playerList[i].role = Player.Role.Police;
            }
        }
    }

    void SetAllRoles(Player.Role _role){
        for (int i = 0; i < playerList.Count; i++) {
            playerList[i].role = _role;
        }
    }

    public void spawnCharacter(Player _player){
        print("Im a playermanager request from: " + _player.OwnerClientId);
        switch (_player.role)
        {
            case Player.Role.Police:
            NetworkManager.SpawnManager.InstantiateAndSpawn(_player.networkPolicePrefab,_player.OwnerClientId,false,true,false,policeSpawnPos.position);
            return;
            case Player.Role.Killer:
            NetworkManager.SpawnManager.InstantiateAndSpawn(_player.networkKillerPrefab,_player.OwnerClientId,false,true,false,KillerSpawnPos.position);
            return;
        }
    }

    public void SpawnAllCharacters(){
        foreach (Player player in playerList){
            spawnCharacter(player);
        }
    }
}
