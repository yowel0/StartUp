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
        //setRoleRpc();
        //spawnCharacterRpc(OwnerClientId,role);
    }

    [Rpc(SendTo.Server)]
    void spawnCharacterRpc()
    {
        PlayerManager.instance.spawnCharacter(this);
    }
}
