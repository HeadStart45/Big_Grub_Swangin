using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float JumpPower;
    [SerializeField] private Rigidbody rb;

    private bool FirstJump = false;
    
    private bool isTouchingVine;
    private Vine TouchedVine;

    private Coroutine delayReleaseCoro;
    // Start is called before the first frame update


    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!FirstJump)
            {
                rb.AddForce(new Vector3(0.5f, 0.5f, 0) * JumpPower, ForceMode.Impulse);
                FirstJump = true;
            }
            else if (isTouchingVine)
            {
                TouchedVine.Link(rb);
                rb.AddForce(rb.velocity * 10.0f);
            }
            
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (isTouchingVine)
            {
                TouchedVine.DeLink();
                isTouchingVine = false;
                TouchedVine = null;

                rb.AddForce(new Vector3(0.5f, 0.5f, 0) * 4, ForceMode.Impulse);

            }
        }

        

    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, rb.velocity.normalized, out hit, 4.0f))
        {
            if (hit.transform.gameObject.CompareTag("Vine"))
            {
                if (delayReleaseCoro != null)
                {
                    StopCoroutine(delayReleaseCoro);
                }
                isTouchingVine = true;
                TouchedVine = hit.transform.gameObject.GetComponent<Vine>();
            }
            Debug.DrawRay(transform.position, rb.velocity.normalized * hit.distance, Color.green);
            Debug.Log("HitVine");
        }
        //Debug.DrawRay(transform.position, rb.velocity.normalized, Color.yellow);
    }

    private void OnCollisionEnter(Collision _collision)
    {
        
    }

    private void OnCollisionExit(Collision _collision)
    {
        if (_collision.gameObject == TouchedVine)
        {
            Debug.Log("EndContact");
            delayReleaseCoro = StartCoroutine(DelayReleaseVine());
            
        }
    }


    IEnumerator DelayReleaseVine()
    {
        yield return new WaitForSeconds(0.1f);
        isTouchingVine = false;
        TouchedVine = null;
    }
}
