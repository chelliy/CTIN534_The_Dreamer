using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
public class Door : MonoBehaviour
{
    [SerializeField] private BoxButton button;
    public void ToggleDoor()
    {
        this.gameObject.SetActive(!button.isPressed); 
    }
}
