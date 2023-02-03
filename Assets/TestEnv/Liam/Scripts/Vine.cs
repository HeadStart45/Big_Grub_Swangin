using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Vine : MonoBehaviour
{
    private FixedJoint tempJoint;

    [SerializeField] private HingeJoint hJoint;
    
    [SerializeField]
    private bool OffSwing;
    public void Link(Rigidbody _rbToLink)
    {
        tempJoint = gameObject.AddComponent<FixedJoint>();

        tempJoint.connectedBody = _rbToLink;
        
    }

    private void FixedUpdate()
    {
        
    }

    public void DeLink()
    {
        Destroy(tempJoint);
        tempJoint = null;
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }
}
