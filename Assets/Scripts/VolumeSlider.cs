using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;


public class VolumeSlider : MonoBehaviour
{
    private enum VolumeType
    {
        UI,

        AMBIENCE,

        SFX
    }

    [Header("Type")]
    [SerializeField] private VolumeType volumeType;

    private Slider volumeSlider;

    private void Awake()
    {
        volumeSlider = this.GetComponentInChildren<Slider>();
    }

    private void Update()
    {
        switch (volumeType)
        {
            case VolumeType.UI:
                volumeSlider.value = AudioControls.uiVolume;
                break;
            case VolumeType.AMBIENCE:  
                volumeSlider.value = AudioControls.ambienceVolume;
                break;
            case VolumeType.SFX:
                volumeSlider.value = AudioControls.sfxVolume;
                break;

            default:
                Debug.LogWarning("Volumetype not supported: " +  volumeType); break;
        }
    }

    public void OnSliderChange()
    {
        switch (volumeType)
        {
            case VolumeType.UI:
                AudioControls.uiVolume = volumeSlider.value;
                break;
            case VolumeType.AMBIENCE:
                AudioControls.ambienceVolume = volumeSlider.value;
                break;
            case VolumeType.SFX:
                AudioControls.sfxVolume = volumeSlider.value;
                break;

            default:
                Debug.LogWarning("Volumetype not supported: " + volumeType); break;
        }
    }
}
