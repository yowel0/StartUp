using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using Mono.Cecil;
public class FMODEvents : MonoBehaviour
{

    [field: Header("Player Footsteps")]
    [field: SerializeField] public FMODUnity.EventReference doFootsteps { get; private set; }

    [field: Header("Photograph")]
    [field: SerializeField] public FMODUnity.EventReference takePhotograph { get; private set; }

    public static FMODEvents instance {  get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one audiomanager found in scene!!!!!");
        }
        instance = this;
    }

}
