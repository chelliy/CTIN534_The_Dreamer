using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
    [SerializeField] private GameObject player; 
    [SerializeField] private GameObject spawnPosition; 
    private void OnTriggerEnter(Collider other)
    {
        player.transform.position = spawnPosition.transform.position; 
    }
}
