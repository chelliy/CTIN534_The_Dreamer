using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObj : MonoBehaviour
{
    // Start is called before the first frame update

    public bool realityObj = false;
    private bool onHold = false;

    public Vector3 orginalPosition;

    void Start()
    {
        orginalPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetOnHoldStatus(bool isHold)
    {
        onHold = isHold;
    }

    public void TeleportCheck()
    {
        //if (!realityObj) 
        //{
        //    if (onHold)
        //    {
        //        PickUpScript.pickUpControl.DropObject();
        //        onHold = false;
        //    }
        //}
    }

    public void ResetPosition()
    {
        this.transform.position = orginalPosition;
    }
}
