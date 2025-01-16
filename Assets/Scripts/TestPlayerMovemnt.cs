using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMovemnt : MonoBehaviour
{
    [SerializeField]
    float moveSpeed = 4;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * Time.deltaTime * moveSpeed;
        transform.position += new Vector3(inputVec.x,0,inputVec.y);
    }
}
