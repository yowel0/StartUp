using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MurderLight : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (IsOwner)
        {

            this.GetComponent<Light>().enabled = false;

        }
    }
}
