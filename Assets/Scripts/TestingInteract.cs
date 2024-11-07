using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingInteract : MonoBehaviour
{
    // Start is called before the first frame update
    public Outline m_outline;
    void Start()
    {
        m_outline = GetComponent<Outline>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        Debug.Log("Happended");
        if (m_outline)
        {
            m_outline.enabled = true;
        }
    }

    private void OnMouseExit()
    {
        if (m_outline)
        {
            m_outline.enabled = false;
        }

    }

}
