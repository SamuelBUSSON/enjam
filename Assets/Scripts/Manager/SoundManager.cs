using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager instance;

    private float currentVolume = 0.0f;

    public bool isMusicPlaying = false;

    void Awake()
    {
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        else
        {
            //If instance already exists and it's not this:
            if (instance != this)
            {
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
            }
        }
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);        
    }

    public void SetCurrentVolume(float newVol)
    {
        currentVolume = newVol;
    }

    public float GetCurrentVolume()
    {
        return currentVolume;
    }
}
