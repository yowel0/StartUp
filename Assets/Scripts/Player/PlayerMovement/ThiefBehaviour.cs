using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThiefBehaviour : MoveBehaviour
{
    [SerializeField] float crouchMult;


    public override void StateHandler()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            state = States.crouching;
            speedMult = crouchMult;
        }
        else base.StateHandler();
    }
}
