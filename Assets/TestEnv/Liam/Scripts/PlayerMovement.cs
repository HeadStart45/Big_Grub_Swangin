using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float JumpPower;
    [SerializeField] private float ReleaseVinePower;
    [SerializeField] private float GrabSafeZone;
    [SerializeField] private Vector3 JumpDirection;
    
    //Components
    [SerializeField] private Rigidbody rb;

    private bool FirstJump = false;
    
    private bool isTouchingVine;
    private Vine TouchedVine;

    private Vine GrabbedVine;
    

    private Coroutine delayReleaseCoro;
    // Start is called before the first frame update


    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!FirstJump)
            {
                rb.AddForce(JumpDirection * JumpPower, ForceMode.Impulse);
                FirstJump = true;
            }
            else if (isTouchingVine)
            {
                GrabVine();
            }
            
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (isTouchingVine)
            {
                ReleaseVine();
            }
        }

        

    }

    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, rb.velocity.normalized, out hit, GrabSafeZone))
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

    private void GrabVine()
    {
        TouchedVine.Link(rb);
        GrabbedVine = TouchedVine;
        TouchedVine = null;
        isTouchingVine = false;
        //rb.AddForce(rb.velocity * ReleaseVinePower, ForceMode.Impulse);
    }

    private void ReleaseVine()
    {
        GrabbedVine.DeLink();
        isTouchingVine = false;
        TouchedVine = null;

        rb.AddForce(JumpDirection * ReleaseVinePower, ForceMode.Impulse);
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
