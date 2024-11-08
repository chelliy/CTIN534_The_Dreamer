using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInteract2 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject lastOne;
    public GameObject currentOne;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity, 1<<LayerMask.NameToLayer("ConstantPickUp"));
        if(hits.Length > 0)
        {
            RaycastHit current = hits[hits.Length -1];
            currentOne = current.transform.gameObject;
            if (lastOne)
            {
                lastOne.GetComponent<Outline>().enabled = false;
                lastOne = null;
            }
            currentOne.GetComponent<Outline>().enabled = true;
            lastOne = currentOne;
        }
        else
        {
            if (lastOne)
            {
                lastOne.GetComponent<Outline>().enabled = false;
                lastOne = null;
            }
        }
    }
}
