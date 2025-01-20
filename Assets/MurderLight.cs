using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MurderLight : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (this.GetComponent<ThiefBehaviour>() == null)
        {

            this.GetComponent<Light>().enabled = false;

        }
    }
}
