using System.Collections;
using GameManagers;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float JumpPower;
    [SerializeField] private float ReleaseVinePower;
    [SerializeField] private float FartPower;
    [SerializeField] private float MushroomPower;
    [SerializeField] private float GrabVinePower;
    [SerializeField] private float GrabSafeZone;
    [SerializeField] private Vector3 JumpDirection;
    [SerializeField] private Vector3 ReleaseDirection;


    private int FartsPossible = 3;
    private float initialX;

    //Components
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator an;

    private bool FirstJump = true;

    [SerializeField]
    private Vine TouchedVine;
    [SerializeField]
    private Vine GrabbedVine;
    
    //Joint Holding the character to vine
    
    private FixedJoint tempJoint;
    
    // Start is called before the first frame update
    private void Start()
    {
        initialX = transform.position.x;
        
    }

    // Update is called once per frame
    private void Update()
    {
        // Mobile device, so get touch controls
        if (Application.isMobilePlatform) {
            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (!FirstJump)
                {
                    Debug.Log("Jump");
                    rb.AddForce(JumpDirection * JumpPower, ForceMode.Impulse);
                    FirstJump = true;
                }
                else if (TouchedVine != null)
                {
                    GrabVine();
                }
            }

            if (touch.phase != TouchPhase.Ended) {
                return;
            }

            if (GrabbedVine != null) {
                ReleaseVine();
            }

            // Early return to not test for keyboard input
            return;
        }
        //Fart works on Alt only on keyboard, fart button on screen for mobile
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Fart();
        }


        // GameMan.Instance.IncrementScore(-(transform.position.x - initialX));

        // Keyboard input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (TouchedVine != null)
            {
                GrabVine();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (GrabbedVine != null)
            {
                ReleaseVine();
            }


        }

        if (GameMan.Instance.Testing)
        {
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(-0.1f, 0f, 0f);
            }
        }



    }

    private void FixedUpdate()
    {
        /*
        if (GrabbedVine == null)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, rb.velocity.normalized, out hit, GrabSafeZone))
            {
                if (hit.transform.gameObject.CompareTag("Vine") && GrabbedVine == null)
                {
                    Debug.Log("HitVine");
                    TouchedVine = hit.transform.GetComponent<Vine>();
                    //Debug.DrawRay(transform.position, rb.velocity.normalized * hit.distance, Color.green)
                }
            }
        }
        */
        //Debug.DrawRay(transform.position, rb.velocity.normalized, Color.yellow);
    }

    private void GrabVine()
    {
        tempJoint = gameObject.AddComponent<FixedJoint>();
        tempJoint.connectedBody = TouchedVine.GetComponent<Rigidbody>();
        GrabbedVine = TouchedVine;
    
        rb.AddForce(ReleaseDirection * GrabVinePower, ForceMode.Impulse);
    }

    private void ReleaseVine()
    {
        Destroy(tempJoint);
        Destroy(GrabbedVine.gameObject.GetComponent<BoxCollider>());
        GrabbedVine = null;
        TouchedVine = null;

        rb.AddForce(JumpDirection * ReleaseVinePower, ForceMode.Impulse);
    }

    private void Fart()
    {
        if (FartsPossible > 0)
        {
            FartsPossible--;
            rb.AddForce(JumpDirection*FartPower, ForceMode.Impulse);
            an.SetTrigger("Fart");
            Debug.Log("Fart");
        }
        
    }

    private void OnTriggerExit(Collider _collision)
    {
        if (_collision.gameObject == TouchedVine)
        {
            Debug.Log("EndContact");

                TouchedVine = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Vine"))
        {
            TouchedVine = other.gameObject.GetComponent<Vine>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
 
        if (collision.gameObject.CompareTag("Mushroom"))
        {
            Debug.Log("Mushroom Blast");
            rb.AddForce(JumpDirection * MushroomPower, ForceMode.Impulse);
        }
        else
        {
            if (!GameMan.Instance.Testing && collision.gameObject.CompareTag("Ground"))
            {
                GameMan.Instance.PlayerHasDied();
                Destroy(this.gameObject);
            }

        }
            

        
    }

    IEnumerator DelayReTouch(Vine _vine)
    {
        BoxCollider coll = _vine.GetComponent<BoxCollider>();
        coll.enabled = false;
        yield return new WaitForSeconds(1.0f);
        coll.enabled = true;
    }

}
