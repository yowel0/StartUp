using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartBeat : MonoBehaviour
{
    AudioSource audio;

    List<Transform> copsInRadius = new List<Transform>();
    Transform closestCop;
    bool policeClose = false;
    [SerializeField] float distClosestCop = 100;
    bool isPLaying = false;


    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }
    private void Update()
    {
        HeartBeating();
    }
    void HeartBeating()
    {
        if (policeClose)
        {
            for (int i = 0; i < copsInRadius.Count; i++)
            {
                float dist = (copsInRadius[i].position - transform.position).magnitude;
                if (dist < distClosestCop || closestCop == null)
                {
                    closestCop = copsInRadius[i];
                }
            }
            distClosestCop = (closestCop.position - transform.position).magnitude;
            audio.volume = 2 - (distClosestCop / 10);

            //Add a script to change the speed/volume of the heartbeat audio based on the distClosestCop.
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Police"))
        {
            policeClose = true;
            copsInRadius.Add(other.transform);
            if (!isPLaying)
            {
                audio.Play();
                isPLaying = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Police"))
        {
            if (other.transform == closestCop)
                closestCop = null;

            copsInRadius.Remove(other.transform);
            if (copsInRadius.Count == 0)
            {
                audio.Stop();
                isPLaying = false;
                policeClose = false;
            }
        }
    }
}
