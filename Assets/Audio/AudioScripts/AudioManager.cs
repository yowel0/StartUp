using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public GameObject whatThePlayer;
    private EventInstance ambienceEventInstance;
    public static AudioManager instance { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one audiomanager found in scene!!!!!");
        }
        instance = this;
    }

    private void Start()
    {
        InitializeAmbience(FMODEvents.instance.hallAmbinece);
        InitializeAmbience(FMODEvents.instance.roomAmbience);
        InitializeAmbience(FMODEvents.instance.kitchenAmbience);
        InitializeAmbience(FMODEvents.instance.bathAmbience);
       
   
    }
    private void InitializeAmbience(EventReference ambienceEventReference)
    {
        ambienceEventInstance = RuntimeManager.CreateInstance(ambienceEventReference);

        
        ambienceEventInstance.start();
    }
    private void Update()
    {
        if (ambienceEventInstance.isValid())
        {
            // Continuously update the listener's position
            var listenerPosition = Camera.main.transform.position; // Example: using the main camera's position
            ambienceEventInstance.set3DAttributes(RuntimeUtils.To3DAttributes(listenerPosition));
        }
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public void setAmbienceParameter(string parameterName, float parameterValue)
    {
        ambienceEventInstance.setParameterByName(parameterName, parameterValue);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }



}

