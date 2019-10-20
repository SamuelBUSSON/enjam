using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeUI : MonoBehaviour
{

    public float jumpTimerPair = 0.5f;
    public float jumpTimerImpair = 1.0f;
    public float endTimer = 1.5f;

    [HideInInspector()]
    public float timer = 0.0f;

    public Sprite imageOn;
    public Sprite imageOff;

    public bool goTimerPair = false;
    public bool goTimerImpair = false;

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= jumpTimerPair && timer <= jumpTimerImpair)
        {
            goTimerPair = true;
        }
        if (timer >= jumpTimerImpair && timer  <= endTimer)
        {
            goTimerImpair = true;
        }

        if(timer >= endTimer)
        {
            goTimerPair = false;
            goTimerImpair = false;
            timer = 0.0f;
        }
    }
}
