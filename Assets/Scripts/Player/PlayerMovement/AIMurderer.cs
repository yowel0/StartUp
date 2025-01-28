using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMurderer : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject player;
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    [SerializeField] List<GameObject> walkTo = new List<GameObject>();
    int walkToInt = 0;
    public LayerMask whatIsGround;

    bool grabbed;


    Rigidbody rb;
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }


    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", 1);
        
        if (grabbed) agent.isStopped = true;
        else Patroling();

    }
    private void Patroling()
    {
        agent.isStopped = false;
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);
        agent.transform.LookAt(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 2f)
            walkPointSet = false;
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

}

