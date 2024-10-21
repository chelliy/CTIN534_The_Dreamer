using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealityDreamControl : MonoBehaviour
{
    // Start is called before the first frame update
    public static RealityDreamControl realityDreamControl;
    public bool canSwitchBetweenRD = true;
    private bool inReality = true;
    public GameObject realityPosition;
    public GameObject dreamPosition;
    public GameObject player;

    void Start()
    {
        realityDreamControl = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSwitchBetweenRD)
        {
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                SwitchRealityDream();
                player.GetComponent<FirstPersonController>().Teleported();
            }
        }
    }

    public void SwitchRealityDream()
    {
        inReality = !inReality;
        if (PickUpScript.pickUpControl.heldObj)
        {
            PickUpScript.pickUpControl.heldObj.GetComponent<InteractableObj>().TeleportCheck();
        }
        if (inReality)
        {
            player.transform.position = realityPosition.transform.position;
        }
        else
        {
            player.transform.position = dreamPosition.transform.position;
        }
    }

    public void SetSwitchable(bool switchable)
    {
        canSwitchBetweenRD = switchable;
    }
}
