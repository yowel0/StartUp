using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class FMODEvents : MonoBehaviour
{

    [field: Header("Photograph")]
    [field: SerializeField] public FMODUnity.EventReference takePhotograph { get; private set; }

    [field: Header("Footsteps")]
    [field: SerializeField] public FMODUnity.EventReference doFootsteps { get; private set; }

    [field: Header("hallAmbience")]
    [field: SerializeField] public FMODUnity.EventReference hallAmbinece { get; private set; }

    [field: Header("RoomAmbience")]
    [field: SerializeField] public FMODUnity.EventReference roomAmbience { get; private set; }

    [field: Header("KitchenAmbience")]
    [field: SerializeField] public FMODUnity.EventReference kitchenAmbience { get; private set; }

    [field: Header("BathroomAmbience")]
    [field: SerializeField] public FMODUnity.EventReference bathAmbience { get; private set; }

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
