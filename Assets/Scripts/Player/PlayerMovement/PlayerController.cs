using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using System.Reflection.Emit;

[RequireComponent(typeof(MoveBehaviour))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private MoveBehaviour moveBehaviour;


    private void Start()
    {
        moveBehaviour = GetComponent<MoveBehaviour>();
    }
}
