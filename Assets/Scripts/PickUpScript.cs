﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos;
    //if you copy from below this point, you are legally required to like the video
    public float throwForce = 500f; //force at which the object is thrown at
    public float pickUpRange = 5f; //how far the player can pickup the object from
    private float rotationSensitivity = 1f; //how fast/slow the object is rotated in relation to mouse movement
    public GameObject heldObj; //object which we pick up
    private Rigidbody heldObjRb; //rigidbody of object we pick up
    private bool canDrop = true; //this is needed so we don't throw/drop object when rotating the object
    private int LayerNumber; //layer index

    //Reference to script which includes mouse movement of player (looking around)
    //we want to disable the player looking around when rotating the object
    //example below 
    FirstPersonController firstPersonControllerScript;
    float originalvalue = 0f;
    int heldOriginLayer = 0;

    bool foundPickable = false;
    GameObject potentialHeldObj = null;

    public static PickUpScript pickUpControl;
    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("holdLayer"); //if your holdLayer is named differently make sure to change this ""
        pickUpControl = this;
        firstPersonControllerScript = player.GetComponent<FirstPersonController>();
        originalvalue = firstPersonControllerScript.mouseSensitivity;
    }
    void Update()
    {
        RaycastHit [] hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), pickUpRange, 1 << LayerMask.NameToLayer("RealitySide") | 1 << LayerMask.NameToLayer("DreamSide") | 1 << LayerMask.NameToLayer("ConstantPickUp"));
        
        if (hits.Length > 0)
        {
            RaycastHit hit = hits[0];
            if (heldObj == null)
            {
                //make sure pickup tag is attached
                if (hit.transform.gameObject.tag == "canPickUp")
                {
                    //pass in object hit into the PickUpObject function
                    potentialHeldObj = hit.transform.gameObject;
                    potentialHeldObj.GetComponent<Outline>().enabled = true;
                    foundPickable = true;

                }
                else
                {
                    if (foundPickable)
                    {
                        potentialHeldObj.GetComponent<Outline>().enabled = false;
                        potentialHeldObj = null;
                        foundPickable = false;
                    }
                }
            }
        }
        else
        {
            if (foundPickable)
            {
                potentialHeldObj.GetComponent<Outline>().enabled = false;
                potentialHeldObj = null;
                foundPickable = false;
            }

        }
        if (Input.GetKeyDown(KeyCode.E)) //change E to whichever key you want to press to pick up
        {
            if (heldObj == null) //if currently not holding anything
            {
                //perform raycast to check if player is looking at object within pickuprange
                if (foundPickable)
                {
                    //pass in object hit into the PickUpObject function
                    PickUpObject(potentialHeldObj);
                }
                
            }
            else
            {
                if(canDrop == true)
                {
                    StopClipping(); //prevents object from clipping through walls
                    DropObject();
                }
            }
        }
        if (heldObj != null) //if player is holding object
        {
            MoveObject(); //keep object position at holdPos
            RotateObject();
            if (Input.GetKeyDown(KeyCode.Mouse0) && canDrop == true) //Mous0 (leftclick) is used to throw, change this if you want another button to be used)
            {
                StopClipping();
                ThrowObject();
            }

        }
    }
    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>()) //make sure the object has a RigidBody
        {
            heldObj = pickUpObj; //assign heldObj to the object that was hit by the raycast (no longer == null)
            heldObjRb = pickUpObj.GetComponent<Rigidbody>(); //assign Rigidbody
            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform; //parent object to holdposition
            heldOriginLayer = heldObj.layer;
            heldObj.layer = LayerNumber; //change the object layer to the holdLayer
            //make sure object doesnt collide with player, it can cause weird bugs
            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
            pickUpObj.GetComponent<InteractableObj>().SetOnHoldStatus(true);

            potentialHeldObj.GetComponent<Outline>().enabled = false;
            potentialHeldObj = null;
            foundPickable = false;
        }
    }
    public void DropObject()
    {
        //re-enable collision with player
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = heldOriginLayer; //object assigned back to default layer
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null; //unparent object
        heldObj = null; //undefine game object
    }
    void MoveObject()
    {
        //keep object position the same as the holdPosition position
        heldObj.transform.position = holdPos.transform.position;
    }
    void RotateObject()
    {
        if (Input.GetKey(KeyCode.R))//hold R key to rotate, change this to whatever key you want
        {
            canDrop = false; //make sure throwing can't occur during rotating

            //disable player being able to look around
            firstPersonControllerScript.mouseSensitivity = 0f;
            firstPersonControllerScript.mouseSensitivity = 0f;

            float XaxisRotation = Input.GetAxis("Mouse X") * rotationSensitivity;
            float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSensitivity;
            //rotate the object depending on mouse X-Y Axis
            heldObj.transform.Rotate(Vector3.down, XaxisRotation);
            heldObj.transform.Rotate(Vector3.right, YaxisRotation);
        }
        else
        {
            //re-enable player being able to look around
            firstPersonControllerScript.mouseSensitivity = originalvalue;
            firstPersonControllerScript.mouseSensitivity = originalvalue;
            canDrop = true;
        }
    }
    void ThrowObject()
    {
        //same as drop function, but add force to object before undefining it
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = heldOriginLayer;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObjRb.AddForce(transform.forward * throwForce);
        heldObj = null;
    }
    void StopClipping() //function only called when dropping/throwing
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); //distance from holdPos to the camera
        //have to use RaycastAll as object blocks raycast in center screen
        //RaycastAll returns array of all colliders hit within the cliprange
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
        //if the array length is greater than 1, meaning it has hit more than just the object we are carrying
        if (hits.Length > 1)
        {
            if (!Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), clipRange, 1 << LayerMask.NameToLayer("Stasis")))
            {
                //change object position to camera position 
                heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f); //offset slightly downward to stop object dropping above player 
                                                                                              //if your player is small, change the -0.5f to a smaller number (in magnitude) ie: -0.1f
            }
        }
    }
}
