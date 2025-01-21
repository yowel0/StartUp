using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class StartRoundButton : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Rpc(SendTo.Server)]
    public void StartRoundRpc(){
        PlayerManager.instance.StartRound();
        print("serverskiii");
        Destroy();
    }

    public void Destroy(){
        Destroy(gameObject);
    }
}
