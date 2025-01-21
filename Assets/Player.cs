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
    public NetworkObject networkPolicePrefab;
    public NetworkObject networkKillerPrefab;

    public NetworkObject networkSphere;

    private GameObject playerPrefab;

    public override void OnNetworkSpawn()
    {
        if(!IsOwner){
            return;
        }
        base.OnNetworkSpawn();
        setRoleRpc();
        spawnCharacterRpc(OwnerClientId,role);
    }

    [Rpc(SendTo.Server)]
    void setRoleRpc(){
        if(PlayerManager.instance.playerList.Count == 0){
            role = Role.Killer;
        }
        else{
            role = Role.Police;
        }
    }

    [Rpc(SendTo.Server)]
    void spawnCharacterRpc(ulong _OwnerClientId,Player.Role _Role){
        switch (_Role){
            case Role.Police:
            GameObject.FindObjectOfType<PlayerManager>().spawnSphere(_OwnerClientId, networkPolicePrefab);
            return;
            case Role.Killer:
            GameObject.FindObjectOfType<PlayerManager>().spawnSphere(_OwnerClientId, networkKillerPrefab);
            return;
        }
    }

    // [Rpc(SendTo.Server)]
    // void SpawnPlayerRpc(Role _role){
    //     switch(_role){
    //         case Role.Police:
    //         playerPrefab = Instantiate(policePrefab,transform);
    //         return;
    //         case Role.Killer:
    //         playerPrefab = Instantiate(killerPrefab,transform);
    //         return;
    //     }
    //     //NetworkManager.Singleton.AddNetworkPrefab(playerPrefab);
    //     GameObject player = Instantiate(playerPrefab,transform);
    //     NetworkObject playerNetworkObject = player.GetComponent<NetworkObject>();
    //     playerNetworkObject.SpawnWithOwnership(OwnerClientId);

    //     //NetworkManager.SpawnManager.InstantiateAndSpawn(playerPrefab.GetComponent<NetworkObject>(),OwnerClientId);
    //     //playerPrefab.ChangeOwnership(this.OwnerClientId);
    // }

    // void SpawnPlayer(Role _role){
    //     switch(_role){
    //         case Role.Police:
    //         playerPrefab = Instantiate(policePrefab,transform);
    //         return;
    //         case Role.Killer:
    //         playerPrefab = Instantiate(killerPrefab,transform);
    //         return;
    //     }
    //     //NetworkManager.Singleton.AddNetworkPrefab(playerPrefab);
    //     GameObject player = Instantiate(playerPrefab,transform);
    //     NetworkObject playerNetworkObject = player.GetComponent<NetworkObject>();
    //     playerNetworkObject.SpawnWithOwnership(OwnerClientId);

    //     //NetworkManager.SpawnManager.InstantiateAndSpawn(playerPrefab.GetComponent<NetworkObject>(),OwnerClientId);
    //     //playerPrefab.ChangeOwnership(this.OwnerClientId);
    // }
}
