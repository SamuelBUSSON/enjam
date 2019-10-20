using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoomBox : MonoBehaviour
{
    public float minVolume = 0.0f;
    public float maxVolume = 100.0f;

    public float speedVolume = 20.0f;

    public float currentVolume;

    public Slider sliderVolume;

    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        currentVolume = minVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {

        }
        if (Input.GetKey(KeyCode.RightArrow))
        {

        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            DecreaseVolume();
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            IncreaseVolume();
        }
    }

    public float GetPercentVolume()
    {
        return currentVolume / maxVolume;
    }

    private void IncreaseVolume()
    {
        currentVolume += speedVolume * Time.deltaTime;
        if(currentVolume >= maxVolume)
        {
            currentVolume = maxVolume;
        }
        sliderVolume.value = GetPercentVolume();
    }

    private void DecreaseVolume()
    {
        currentVolume -= speedVolume * Time.deltaTime;
        if(currentVolume <= minVolume)
        {
            currentVolume = minVolume;
        }
        sliderVolume.value = GetPercentVolume();
    }
}
