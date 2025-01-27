using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using UnityEngine.AI;
using System.Reflection.Emit;
using Unity.Netcode.Components;

[RequireComponent(typeof(MoveBehaviour))]
public class PlayerController : NetworkBehaviour
{
    [SerializeField]
    private MoveBehaviour moveBehaviour;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsOwner){
            this.enabled = false;
        }
    }

    private void Start()
    {
        moveBehaviour = GetComponent<MoveBehaviour>();
    }
}
