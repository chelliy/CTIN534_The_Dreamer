using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events; 
public class BoxButton : MonoBehaviour
{
    public bool isPressed = false;
    public UnityEvent buttonEvent; 
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "canPickUp")
        {
            isPressed = true;
            buttonEvent.Invoke(); 
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "canPickUp")
        {
            isPressed = false;
            buttonEvent.Invoke(); 
        }
    }
}
