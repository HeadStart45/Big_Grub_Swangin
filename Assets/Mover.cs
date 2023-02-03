using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = new Vector3(transform.position.x, transform.position.y,
                transform.position.z + (3.0f * Time.deltaTime));
        };
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collide");
    }
}
