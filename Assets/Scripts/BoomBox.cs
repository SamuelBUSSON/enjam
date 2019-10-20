using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoomBox : MonoBehaviour
{
    [Header("Volume")]
    public float minVolume = 0.0f;
    public float maxVolume = 100.0f;
    public float speedVolume = 20.0f;

    [Header("Detection Zone")]
    public float minRange = 5.0f;
    public float maxRange = 10.0f;


    private float currentVolume;
    private AudioSource source;
    private SphereCollider triggerZone;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        currentVolume = minVolume;
        triggerZone = GetComponent<SphereCollider>();
        animator = GetComponent<Animator>();
        UpdateVal();
    }

    // Update is called once per frame
    void Update()
    {
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
        UpdateVal();
    }

    private void DecreaseVolume()
    {
        currentVolume -= speedVolume * Time.deltaTime;
        if(currentVolume <= minVolume)
        {
            currentVolume = minVolume;
        }
        UpdateVal();
    }

    public void UpdateVal()
    {
        triggerZone.radius = Mathf.Lerp(minRange, maxRange, GetPercentVolume());
        AkSoundEngine.SetRTPCValue("volume_party", currentVolume);
        AkSoundEngine.SetRTPCValue("volume_idle",  currentVolume);

        if (GetPercentVolume() <= 0.3f)
        {
            animator.SetBool("IsHidding", true);
            animator.SetBool("IsDancing", false);

            FlockingManager.instance.SetAllAgentDiscret();
        }
        else
        {
            animator.SetBool("IsHidding", false);
            animator.SetBool("IsDancing", true);

            FlockingManager.instance.SetAllAgentDancing();
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Wall>())
        {
            other.GetComponent<Wall>().SetShakeTheWall(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Wall>())
        {
            other.GetComponent<Wall>().SetShakeTheWall(false);
        }
    }
}
