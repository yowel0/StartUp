using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioControls : MonoBehaviour
{
    [Header("volume")]
    [Range(0, 1)]
    public static float uiVolume = 1;
    [Range(0, 1)]
    public static float ambienceVolume = 1;
    [Range(0, 1)]
    public static float sfxVolume = 1;

    private Bus uiBus;
    private Bus ambienceBus;
    private Bus sfxBus;


    // Start is called before the first frame update
    void Awake()
    {
        uiBus = RuntimeManager.GetBus("bus:/UI");
        ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
    }

    // Update is called once per frame
    void Update()
    {
        uiBus.setVolume(uiVolume);
        ambienceBus.setVolume(ambienceVolume);
        sfxBus.setVolume(sfxVolume);
    }
}
