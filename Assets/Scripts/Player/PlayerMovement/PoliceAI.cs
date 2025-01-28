using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PoliceAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject player;
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    [SerializeField] List<GameObject> walkTo = new List<GameObject>();
    int walkToInt = 0;
    public LayerMask whatIsGround;

    public bool grabbed = false;
    ThiefBehaviour script;
    InteractionsMurderer script2;
    [SerializeField] GameObject obj = null;
    bool canGrab = true;
    float thiefSpeed;
    [SerializeField] int minShakes;
    [SerializeField] int maxShakes;
    [SerializeField] Transform holdPos;
    [SerializeField] float staggerTime;
    float staggerTimer = 0;
    public bool staggered = false;

    public bool stabbed = false;

    Rigidbody rb;
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Murderer");
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        if (!stabbed)
        {
            animator.SetFloat("Speed", 1);
            Patroling();
            if (grabbed)
            {
                Hold();
                EscapeCheck();
            }
            if (staggered)
            {
                staggerTimer += Time.deltaTime;
                if (staggerTimer > staggerTime)
                {
                    print("ready to jelk");
                    staggered = false;
                }
            }
        }
        else
        {
            animator.SetFloat("Speed", 0);
            transform.rotation = Quaternion.Euler(-90, 0, 0);
            agent.enabled = false;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }
    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 2f)
            walkPointSet = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!staggered && other.CompareTag("Murderer") && other.isTrigger == false && !stabbed && !other.GetComponent<InteractionsMurderer>().hiding)
        {
            obj = other.gameObject;
            Grabbing();
        }
    }

    private void SearchWalkPoint()
    {
        for (int i = walkToInt; i < walkTo.Count; i++)
        {
            walkPoint = walkTo[i].transform.position;
            walkToInt += 1;
            if (walkToInt == walkTo.Count) walkToInt = 0;
            break;
        }
        if (Physics.Raycast(walkPoint, -transform.up, 4f, whatIsGround))
            walkPointSet = true;
    }

    private void Grabbing()
    {
        if (!grabbed)
        {
            script = obj.GetComponent<ThiefBehaviour>();
            script2 = obj.GetComponent<InteractionsMurderer>();
            thiefSpeed = script.speed;
            script.speed = 0;
            script2.grabbed = true;
            script.grabbed = true;
            script2.shakes = UnityEngine.Random.Range(minShakes, maxShakes);


            Rigidbody rig = obj.GetComponent<Rigidbody>();
            rig.useGravity = false;

            CapsuleCollider caps = obj.GetComponent<CapsuleCollider>();
            caps.enabled = false;

            obj.transform.SetParent(holdPos, false);
            print("Rahhh");

            grabbed = true;
        }
    }
    private void LetGo()
    {
        script.speed = thiefSpeed;
        script.grabbed = false;
        script2.grabbed = false;
        Rigidbody rig = obj.GetComponent<Rigidbody>();
        rig.useGravity = true;
        CapsuleCollider caps = obj.GetComponent<CapsuleCollider>();
        caps.enabled = true;
        script = null;
        obj = null;
        canGrab = false;
        grabbed = false;
        staggered = true;
        staggerTimer = 0;
    }
    private void EscapeCheck()
    {
        if (script2.shakes <= 0)
        {
            int esc = UnityEngine.Random.Range(1, 11);
            if (esc >= 1 && esc <= 7)
            {
                LetGo();
            }
            else
            {
                script2.shakes = UnityEngine.Random.Range(minShakes, maxShakes);
            }
        }
    }

    private void Hold()
    {
        obj.transform.position = holdPos.position;
        obj.transform.rotation = holdPos.rotation;
    }

}
