using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StasisZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.CompareTag("canPickUp"))
        {
            other.transform.gameObject.layer = LayerMask.NameToLayer("Default");
            other.excludeLayers = 1 << LayerMask.NameToLayer("Nothing");
            Debug.Log("called");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.gameObject.CompareTag("canPickUp"))
        {
            if (other.transform.gameObject.GetComponent<InteractableObj>().realityObj)
            {
                other.transform.gameObject.layer = LayerMask.NameToLayer("RealitySide");
                other.excludeLayers = 1 << LayerMask.NameToLayer("DreamSide");
            }
            else
            {
                other.transform.gameObject.layer = LayerMask.NameToLayer("DreamSide");
                other.excludeLayers = 1 << LayerMask.NameToLayer("RealitySide");
            }
        }
    }
}
