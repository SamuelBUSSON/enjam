using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusic : MonoBehaviour
{

    uint idMusicIdle;
    uint idMusicParty;

    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        idMusicIdle = AkSoundEngine.PostEvent("Play_music_game_idle", gameObject);
        idMusicParty = AkSoundEngine.PostEvent("Play_music_game_party", gameObject);

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.GetComponentInChildren<BoomBox>().GetPercentVolume() >= 0.6f)
        {
          //  AkSoundEngine.StopPlayingID(idMusicIdle);
        }

    }
}
