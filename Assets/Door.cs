using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 
public class Door : MonoBehaviour
{
    [SerializeField] private BoxButton button;

    public bool special = false;
    public void ToggleDoor()
    {
        if (special)
        {
            this.gameObject.GetComponent<MeshRenderer>().enabled = button.isPressed;
            this.gameObject.GetComponent<MeshCollider>().enabled = button.isPressed;
        }
        else
        {
            this.gameObject.SetActive(!button.isPressed);
        }
    }
}
