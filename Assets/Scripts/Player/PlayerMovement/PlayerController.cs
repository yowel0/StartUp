using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using System.Reflection.Emit;

/// <summary>
/// The only mandatorily required behaviour is MoveBehaviour because other actions are optional.
/// </summary>
[RequireComponent(typeof(MoveBehaviour))]
public class AnimalController : MonoBehaviour
{
    [SerializeField]
    private MoveBehaviour moveBehaviour;


    private void Start()
    {
        moveBehaviour = GetComponent<MoveBehaviour>();
    }
}
