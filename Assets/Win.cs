using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Win : MonoBehaviour
{
    public Canvas winCanvas;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            winCanvas.enabled = true;
            GameManager.instance.currentState = GameManager.State.victory;

            /* Sound */
            AkSoundEngine.PostEvent("Win_end", gameObject);
            AkSoundEngine.PostEvent("Stop_at_the_end", gameObject); 



        }
    }
}
