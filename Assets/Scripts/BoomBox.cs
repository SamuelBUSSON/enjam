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
    private uint idPartySound = 0;


    private float maxTimerParty = 0.5f;
    private float timerParty = 0.0f;

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
        if(GameManager.instance.currentState != GameManager.State.gameOver)
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

        timerParty += Time.deltaTime;

        if(timerParty >= maxTimerParty)
        {
            if (currentVolume >= 60 && FlockingManager.instance.GetNumberOfAgentsInCrew() > 1)
            {
                idPartySound = AkSoundEngine.PostEvent("Prisoniers_happy_voice_in_party", gameObject);
            }
            else
            {
                AkSoundEngine.StopPlayingID(idPartySound);
                idPartySound = 0;
            }
            timerParty = 0.0f;
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
        if (other.GetComponentInParent<FlockingAgent>())
        {
            FlockingAgent fa = other.GetComponentInParent<FlockingAgent>();

            if (!fa.IsInCrew() && fa.volumeNeedToBeHire <= GetPercentVolume() * 100)
            {
                fa.JoinCrew();
            }
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
