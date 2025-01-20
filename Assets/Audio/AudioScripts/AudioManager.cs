using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
<<<<<<< Updated upstream
using FMOD.Studio;
=======
>>>>>>> Stashed changes

public class AudioManager : MonoBehaviour
{
  public static AudioManager instance { get; private set; }
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one audiomanager found in scene!!!!!");
        }
        instance = this;
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }
<<<<<<< Updated upstream
    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }
=======
>>>>>>> Stashed changes
}
