using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 localScale;
    void Start()
    {
        position = transform.position;
        rotation = transform.rotation;
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetTransform()
    {
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = localScale;
    }
}
