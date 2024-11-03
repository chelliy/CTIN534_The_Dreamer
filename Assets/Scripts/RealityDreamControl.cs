using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RealityDreamControl : MonoBehaviour
{
    // Start is called before the first frame update
    public static RealityDreamControl realityDreamControl;
    public bool canSwitchBetweenRD = true;
    public bool automaticSwitch = true;
    [SerializeField] private float automaticSwitchTime = 3.0f; 
    public bool inReality = true;
    //public GameObject realityPosition;
    //public GameObject dreamPosition;
    public GameObject player;

    public String Reality;
    public String Dream;

    public GameObject PostProcess;

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
        if (switchStart)
        {
            if(switchTimer > 0)
            {
                switchTimer-= Time.deltaTime;
            }
            else
            {
                SwitchRealityDream();
                switchTimer = automaticSwitchTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!switchStart)
        {
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
            PostProcess.GetComponent<Volume>().isGlobal = false;
        }
        else
        {
            //player.transform.position = dreamPosition.transform.position;
            Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer(Reality));
            Camera.main.cullingMask |= 1 << LayerMask.NameToLayer(Dream);

            player.GetComponent<Rigidbody>().excludeLayers = 1 << LayerMask.NameToLayer(Reality);

            PostProcess.GetComponent<Volume>().isGlobal = true;
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
