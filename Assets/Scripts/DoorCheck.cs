using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<PoliceBehaviour>() != null)
            {
                if (other.gameObject.GetComponent<PoliceBehaviour>().grabbed == true /*|| evidence collected is enough*/)
                {
                    Debug.Log("end");
                    GameEndSingletons.Instance.EndGame(true);
                }
            }
            else if(other.gameObject.GetComponent<ThiefBehaviour>() != null)
            {
                //if(enough evidence is destroyed){ GameEndSingletons.Instance.EndGame(false) }
            }
        }
    }
}
