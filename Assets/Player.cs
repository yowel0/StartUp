using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public enum Role{
        Police,
        Killer
    }
    public Role role = Role.Killer;
    public GameObject policePrefab;
    public GameObject killerPrefab;

    private GameObject playerPrefab;

    public override void OnNetworkSpawn()
    {
        if(!IsOwner){
            return;
        }
        base.OnNetworkSpawn();
        setRole();
        switch (role){
            case Role.Police:
            // playerPrefab = Instantiate(policePrefab,transform);
            // playerPrefab.Spawn();
            // playerPrefab.ChangeOwnership(this.OwnerClientId);
            SpawnPlayer(role);
            return;
            case Role.Killer:
            // playerPrefab = Instantiate(killerPrefab,transform);
            // playerPrefab.Spawn();
            // playerPrefab.ChangeOwnership(this.OwnerClientId);
            SpawnPlayer(role);
            return;
        }
    }

    void setRole(){
        if(PlayerManager.instance.playerList.Count == 0){
            role = Role.Killer;
        }
        else{
            role = Role.Police;
        }
    }

    [Rpc(SendTo.Server)]
    void SpawnPlayerRpc(Role _role){
        switch(_role){
            case Role.Police:
            playerPrefab = Instantiate(policePrefab,transform);
            return;
            case Role.Killer:
            playerPrefab = Instantiate(killerPrefab,transform);
            return;
        }
        //NetworkManager.Singleton.AddNetworkPrefab(playerPrefab);
        GameObject player = Instantiate(playerPrefab,transform);
        //NetworkObject playerNetworkObject = player.GetComponent<NetworkObject>();
        NetworkManager.SpawnManager.InstantiateAndSpawn(playerPrefab.GetComponent<NetworkObject>(),OwnerClientId);
        //playerPrefab.ChangeOwnership(this.OwnerClientId);
    }

    void SpawnPlayer(Role _role){
        switch(_role){
            case Role.Police:
            playerPrefab = Instantiate(policePrefab,transform);
            return;
            case Role.Killer:
            playerPrefab = Instantiate(killerPrefab,transform);
            return;
        }
        //NetworkManager.Singleton.AddNetworkPrefab(playerPrefab);
        GameObject player = Instantiate(playerPrefab,transform);
        //NetworkObject playerNetworkObject = player.GetComponent<NetworkObject>();
        NetworkObject playerNetworkObject = NetworkManager.SpawnManager.InstantiateAndSpawn(playerPrefab.GetComponent<NetworkObject>(),OwnerClientId);
        playerNetworkObject.Spawn();
        //playerPrefab.ChangeOwnership(this.OwnerClientId);
    }
}
