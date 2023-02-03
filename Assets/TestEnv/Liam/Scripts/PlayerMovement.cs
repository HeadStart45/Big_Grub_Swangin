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
    void Start()
    {
        
    }

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
            }
            
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (isTouchingVine)
            {
                TouchedVine.DeLink();
                isTouchingVine = false;
                TouchedVine = null;

                rb.AddForce((rb.velocity.normalized * 3.0f), ForceMode.Force);

            }
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 3.0f))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("HitVine");
        }

    }

    private void OnCollisionEnter(Collision _collision)
    {
        if (_collision.gameObject.CompareTag("Vine"))
        {
            if (delayReleaseCoro != null)
            {
                StopCoroutine(delayReleaseCoro);
            }
            isTouchingVine = true;
            TouchedVine = _collision.gameObject.GetComponent<Vine>();
        }
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
