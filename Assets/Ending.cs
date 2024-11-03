using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject End_Wall;
    public Image Cover;
    public float counts = -10f;
    void Start()
    {
        Cover.canvasRenderer.SetAlpha(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(counts > 0f)
        {
            counts-= Time.deltaTime;
        }
        else
        {
            if(counts > -10f)
                FadeIn();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            End_Wall.SetActive(true);
            GameEndCount();
        }
    }

    public void GameEndCount()
    {
        counts = 4f;
    }

    public void FadeIn()
    {
        Cover.CrossFadeAlpha(1f, 2f, false);
    }
}
