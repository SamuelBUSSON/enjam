using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicMenu : MonoBehaviour
{

    uint id;

    // Start is called before the first frame update
    void Start()
    {
        id = AkSoundEngine.PostEvent("Play_music_menu", gameObject); 
        
    }

    private void OnDestroy()
    {
        AkSoundEngine.StopPlayingID(id);
    }
}
