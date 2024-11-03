using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    [SerializeField] private GameObject player; 
    [SerializeField] public GameObject currentSpawnPosition; 

    public static DeathPlane deathPlane;

    private void Awake()
    {
        deathPlane = this;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.transform.position = currentSpawnPosition.transform.position;
        }

        if (other.gameObject.CompareTag("canPickUp"))
        {
            other.gameObject.GetComponent<InteractableObj>().ResetPosition();
        }
    }
}
