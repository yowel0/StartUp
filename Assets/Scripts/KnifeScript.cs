using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeScript : MonoBehaviour
{
    public bool grabbed;
    ThiefBehaviour tb;
    BoxCollider box;
    float timer;
    bool rah;
    // Start is called before the first frame update
    void Start()
    {
        tb = GetComponentInParent<ThiefBehaviour>(true);
        box = GetComponentInParent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            box.enabled = true;
            rah = true;
        }
        if (rah)
        {
            timer += Time.deltaTime;
            if(timer >= 1)
            {
                box.enabled = false;
                rah = false;
                timer = 0;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Police") && grabbed && other.isTrigger == false && !tb.grabbed)
        {
            print("stab");
            if (other.gameObject.GetComponent<PoliceBehaviour>() != null)
                other.gameObject.GetComponent<PoliceBehaviour>().enabled = false;
            else if (!other.gameObject.GetComponent<PoliceAi>().staggered)
            {
                other.gameObject.GetComponent<PoliceAi>().stabbed = true;
            }
            Rigidbody rb = other.transform.GetComponent<Rigidbody>();
            rb.isKinematic = false;
            other.transform.rotation = Quaternion.Euler(-90, 0, 0);
        }
    }

}
