using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeAmbienceParameter : MonoBehaviour
{
    [Header("Set parameter")]
    [SerializeField] private string parameterName;
    [SerializeField] private float parameterValue;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            AudioManager.instance.setAmbienceParameter(parameterName, parameterValue);
        }
    }
}
