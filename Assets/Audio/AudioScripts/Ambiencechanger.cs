using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class Ambiencechanger : MonoBehaviour
{
    [Header("parameter change")]
    [SerializeField] private string parameterName;
    [SerializeField] private float parameterValue;


    private void OnTriggerEnter(Collider other)
    {
        if (Collider.tag.Equals("Player"))
        {
            AudioManager.instance.Set(parameterName, parameterValue);
        }
    }

}

