using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;


public class moveFootsteps : MonoBehaviour
{

    private EventInstance playerFootsteps;
    private float speed = 2.0f;
    public GameObject character;


    private void Start()
    {
        playerFootsteps = AudioManager.instance.CreateInstance(FMODEvents.instance.doFootsteps);
    }
    void Update()
    {

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * speed * Time.deltaTime;

            playTheFootsteps();
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            playTheFootsteps();
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.forward * speed * Time.deltaTime;
            playTheFootsteps();
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.back * speed * Time.deltaTime;
            playTheFootsteps();
        }

       void playTheFootsteps()
        {
           
            playerFootsteps.start();
        }
    }

 
}
