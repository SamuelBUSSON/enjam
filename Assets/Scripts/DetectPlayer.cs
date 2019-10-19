using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayer : MonoBehaviour
{

    public float minSoundRange = 3.0f;
    public float maxSoundRange = 5.0f;

    private SphereCollider triggerZone;

    // Start is called before the first frame update
    void Start()
    {
        triggerZone = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        triggerZone.radius = SoundManager.instance.isMusicPlaying ? minSoundRange : maxSoundRange;
    }
}
