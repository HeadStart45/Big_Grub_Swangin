using System;
using System.Collections;
using GameManagers;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float JumpPower;
    [SerializeField] private float ReleaseVinePower;
    [SerializeField] private float FartPower;
    [SerializeField] private float MushroomPower;
    [SerializeField] private float GrabVinePower;

    [SerializeField] private Vector3 JumpDirection;
    [SerializeField] private Vector3 ReleaseDirection;
    [SerializeField] private Vector3 GrabDirection;

    private Rigidbody vineRb = null;
    
    
    private int FartsPossible = 5;
    private float initialX;

    //Components
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Animator an;
    [SerializeField] private ParticleSystem FartParticles;
    [SerializeField] private ParticleSystem JizzParticles;
    [SerializeField] private ParticleSystem WaterParticles;
    [SerializeField] private SoundRandomiser randomFart;
    
    
    private bool FirstJump = true;

    private bool Grabbing = false;

    private GameObject Focus;
    

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
                //GrabVine();
            }

            if (touch.phase != TouchPhase.Ended) {
                ReleaseVine();
                return;
            }

            // Early return to not test for keyboard input
            return;
        }
        //Fart works on Alt only on keyboard, fart button on screen for mobile
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Fart();
        }


        GameMan.Instance.IncrementScore(-(transform.position.x - initialX));

        // Keyboard input
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GrabVine(); 
        }
        
        if (Input.GetKeyUp(KeyCode.Space))
        {
            ReleaseVine(); 
        }
        
        if (GameMan.Instance.Testing)
        {
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(-0.1f, 0f, 0f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsVine(other))
        {
            vineRb = other.GetComponent<Rigidbody>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsVine(other))
        {
            vineRb = null;
        }
    }

    private void GrabVine()
    {
        if (vineRb != null)
        {
            tempJoint = gameObject.AddComponent<FixedJoint>();
            tempJoint.connectedBody = vineRb;
            tempJoint.connectedMassScale = 5;

            JizzParticles.Play();
        
            rb.AddForce(GrabDirection * GrabVinePower, ForceMode.Impulse);
        }
    }

    private void ReleaseVine()
    {
        if (tempJoint != null)
        {
            Destroy(tempJoint);
            rb.AddForce(ReleaseDirection * ReleaseVinePower, ForceMode.Impulse);
            FindObjectOfType<AudioManager>().Play("Jump");
            vineRb = null;
        }
    }

    private void Fart()
    {
        if (FartsPossible > 0)
        {
            FartsPossible--;
            StartCoroutine(FartSpeedActivate());
            Debug.Log("Fart");
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

    IEnumerator FartSpeedActivate()
    {
        an.SetTrigger("Fart");
        transform.rotation = Quaternion.Euler(0, 180, 0);
        randomFart.RandomiseFootstep();
        yield return new WaitForSeconds(0.5f);
        rb.AddForce(JumpDirection*FartPower, ForceMode.Impulse);
        FartParticles.Play();
        GameMan.Instance.fartText.text = FartsPossible.ToString();
    }
    
    bool IsVine(Collider other)
    {
        if (other.GetComponent<Vine>() != null)
        {
            Debug.Log("Is Vine");
            return true;
        }
        else
        {
            return false;
        }
        

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        
    }
}
