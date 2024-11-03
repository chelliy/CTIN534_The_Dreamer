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

    public bool switchStart = false;
    private float switchTimer;


    void Start()
    {
        realityDreamControl = this;
        switchTimer = automaticSwitchTime;
    }

    // Update is called once per frame
    void Update()
    {
        //if (switchStart)
        //{
        //    if(switchTimer > 0f)
        //    {
        //        switchTimer -= Time.deltaTime;
        //    }
        //    else
        //    {
        //        switchTimer = automaticSwitchTime;
        //    }
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!switchStart)
        {
            InvokeRepeating("SwitchRealityDream", automaticSwitchTime, automaticSwitchTime);
            AudioManager.instance.PlayHint();
            switchStart = true;
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
