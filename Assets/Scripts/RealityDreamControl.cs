using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealityDreamControl : MonoBehaviour
{
    // Start is called before the first frame update
    public static RealityDreamControl realityDreamControl;
    public bool canSwitchBetweenRD = true;
    public bool automaticSwitch = true;
    [SerializeField] private float automaticSwitchTime = 3.0f; 
    private bool inReality = true;
    //public GameObject realityPosition;
    //public GameObject dreamPosition;
    public GameObject player;

    public String Reality;
    public String Dream;


    void Start()
    {
        realityDreamControl = this;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        InvokeRepeating("SwitchRealityDream", automaticSwitchTime, automaticSwitchTime);
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
            //player.transform.position = realityPosition.transform.position;
            Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer(Dream));
            Camera.main.cullingMask |= 1 << LayerMask.NameToLayer(Reality);

            player.GetComponent<Rigidbody>().excludeLayers = 1 << LayerMask.NameToLayer(Dream);
        }
        else
        {
            //player.transform.position = dreamPosition.transform.position;
            Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer(Reality));
            Camera.main.cullingMask |= 1 << LayerMask.NameToLayer(Dream);

            player.GetComponent<Rigidbody>().excludeLayers = 1 << LayerMask.NameToLayer(Reality);
        }
    }

    public void SetSwitchable(bool switchable)
    {
        canSwitchBetweenRD = switchable;
    }

    public void SetAutomatic(bool automatic)
    {
        automaticSwitch = automatic; 
    }
}
