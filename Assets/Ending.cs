using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject End_Wall;
    public Image Cover;
    public float counts = -10f;
    public bool reality = false;
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
            if (Cover.canvasRenderer.GetAlpha() >= 0.9) {
                Application.Quit();
            }
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
        RealityDreamControl.realityDreamControl.switchStart = false;
        if (reality) {
            if (!RealityDreamControl.realityDreamControl.inReality) {
                RealityDreamControl.realityDreamControl.SwitchRealityDream();
            }
            RealityDreamControl.realityDreamControl.PostProcess.SetActive(false);
        }
        else
        {
            if (RealityDreamControl.realityDreamControl.inReality)
            {
                RealityDreamControl.realityDreamControl.SwitchRealityDream();
            }
            RealityDreamControl.realityDreamControl.PostProcess.SetActive(true);
            RealityDreamControl.realityDreamControl.PostProcess.GetComponent<Volume>().isGlobal = true;
        }
        AudioManager.instance.StopHint();
    }

    public void FadeIn()
    {
        Cover.CrossFadeAlpha(1f, 1.5f, false);
    }
}
