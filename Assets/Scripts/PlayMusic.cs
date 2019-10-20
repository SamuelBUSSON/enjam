using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{



    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("Play_music_game_idle", gameObject);
        AkSoundEngine.PostEvent("Play_music_game_party", gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
