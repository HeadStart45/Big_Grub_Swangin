<<<<<<< HEAD

=======
using System.Collections;
>>>>>>> c8f0e1a52b3173b4895be2a041a04a10a72779bc
using GameManagers;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    [FormerlySerializedAs("JumpPower")] [SerializeField] private float jumpPower;
    [SerializeField] private float ReleaseVinePower;
    [SerializeField] private float FartPower;
    [SerializeField] private float MushroomPower;
    [SerializeField] private float GrabSafeZone;
    [SerializeField] private Vector3 JumpDirection;

    private float initialX;

    //Components
    [SerializeField] private Rigidbody rb;

    private bool FirstJump = false;

    [SerializeField]
    private Vine TouchedVine;
    [SerializeField]
    private Vine GrabbedVine;

    private FixedJoint tempJoint;

    private Coroutine delayReleaseCoro;
    // Start is called before the first frame update
    private void Start()
    {
        initialX = transform.position.x;
        
    }

    // Update is called once per frame
    private void Update()
    {
        // GameMan.Instance.IncrementScore(-(transform.position.x - initialX));
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!FirstJump)
            {
                Debug.Log("Jump");
                rb.AddForce(JumpDirection * jumpPower, ForceMode.Impulse);
                FirstJump = true;
            }
            else if (TouchedVine != null && CheckIfPlayerIsInVine(TouchedVine))
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

<<<<<<< HEAD
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Fart();
        }
        
=======

>>>>>>> c8f0e1a52b3173b4895be2a041a04a10a72779bc

    }

    private void Fart()
    {
        
        rb.AddForce(JumpDirection * FartPower, ForceMode.Force);
    }

    private void GrabVine()
    {
        tempJoint = gameObject.AddComponent<FixedJoint>();
        tempJoint.connectedBody = TouchedVine.GetComponent<Rigidbody>();
        GrabbedVine = TouchedVine;
<<<<<<< HEAD
        
        rb.AddForce(JumpDirection * 10, ForceMode.VelocityChange);
=======

        //rb.AddForce(rb.velocity * ReleaseVinePower, ForceMode.Impulse);
>>>>>>> c8f0e1a52b3173b4895be2a041a04a10a72779bc
    }

    private void ReleaseVine()
    {
        Destroy(tempJoint);
        Destroy(GrabbedVine.gameObject.GetComponent<BoxCollider>());
        GrabbedVine = null;
        TouchedVine = null;

        rb.AddForce(JumpDirection * ReleaseVinePower, ForceMode.Impulse);
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
        if (FirstJump )
        {
            if (!GameMan.Instance.Testing)
            {
<<<<<<< HEAD
                GameMan.Instance.PlayerHasDied();
                Destroy(this.gameObject);
=======
                Debug.Log("Mushroom Blast");
                rb.AddForce(JumpDirection * MushroomPower, ForceMode.Impulse);
            }
            else
            {
                if (!GameMan.Instance.Testing)
                {
                    GameMan.Instance.PlayerHasDied();
                    Destroy(this.gameObject);
                }

>>>>>>> c8f0e1a52b3173b4895be2a041a04a10a72779bc
            }
        }
        if (collision.gameObject.CompareTag("Mushroom"))
        {
            FirstJump = true;
            
            Debug.Log("Mushroom Blast");
            rb.AddForce(JumpDirection * MushroomPower, ForceMode.Impulse);
        }
        
    }


    private bool CheckIfPlayerIsInVine(Vine _vine)
    {
        return _vine._GraceZone.bounds.Contains(transform.position);
    }

}
