using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events; 
public class BoxButton : MonoBehaviour
{
    public bool isPressed = false;
    public UnityEvent buttonEvent;
    public int counter = 0;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" || other.tag == "canPickUp")
        {
            counter++;
            isPressed = true;
            if (counter <= 1)
            {
                buttonEvent.Invoke();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "canPickUp")
        {
            counter--;
            if(counter <= 0)
            {
                isPressed = false;
                buttonEvent.Invoke(); 
            }
        }
    }
}
