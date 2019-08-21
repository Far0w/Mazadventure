using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Created by LeFarow

public class grabber : MonoBehaviour
{
    //FONCTIONNEMENT :
    //The player touch the grabbable item with his hand, this add the item to lastTouchedObject
    //Then if it trigger the Grab touch on his controller at a small distance (grabThreshold) he will grab the item.


    public bool leftHand = false; // this script is attach to which hand ?
    public bool rightHand = false;
    public float grabThreshold = 1f; // "Small distance"


    private bool isHolding = false; // Is the player holding somthing ?
    private GameObject lastTouchedObject;
    private GameObject heldObject;
    private Rigidbody ActiveRigidBody; // Rigidbody of heldObject

    private void Start()
    {
        if ( (leftHand == false && rightHand == false) || (leftHand == true && rightHand == true) )
        print("ERROR : Grabber Script, select one (and only one) bool : Left OR Right hand");
    }

    private void Update()
    {
        if (isHolding == true)
        {
            if (leftHand && Input.GetAxis("Oculus_CrossPlatform_PrimaryHandTrigger") == 0f)
            {
                dropItem();
            }
            if (rightHand && Input.GetAxis("Oculus_CrossPlatform_SecondaryHandTrigger") == 0f)
            {
                dropItem();
            }
        }
        else
        {
            if (leftHand && (Input.GetAxis("Oculus_CrossPlatform_PrimaryHandTrigger") > 0.5f || Input.GetButton("Oculus_CrossPlatform_PrimaryThumbstick") ) && Vector3.Distance(transform.position, lastTouchedObject.transform.position) < grabThreshold)
            {
                holdItem();
            }
            if (rightHand && (Input.GetAxis("Oculus_CrossPlatform_SecondaryHandTrigger") > 0.5f || Input.GetButton("Oculus_CrossPlatform_SecondaryThumbstick")) && Vector3.Distance(transform.position, lastTouchedObject.transform.position) < grabThreshold)
            {
                holdItem();
            }
        }
    }

    void holdItem()
    {

        heldObject = lastTouchedObject;
        ActiveRigidBody = lastTouchedObject.transform.GetComponent<Rigidbody>();
        ActiveRigidBody.useGravity = false;
        ActiveRigidBody.isKinematic = true;
        heldObject.transform.parent = transform;
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;
        isHolding = true;

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "grabbable_object")
        {
            lastTouchedObject = other.gameObject;
        }
    }
        

    void dropItem()
    {

        ActiveRigidBody.useGravity = true;
        ActiveRigidBody.isKinematic = false;
        heldObject.transform.parent = null;
        isHolding = false;

    }
}
