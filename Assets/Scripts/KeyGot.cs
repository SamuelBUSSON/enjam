using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyGot : MonoBehaviour
{
    private bool obtain;
    private GameObject message;
    // Start is called before the first frame update
    void Start()
    {
        message = GameObject.Find("MessageCanvas");
    }

    void Update()
    {
        if (obtain == true)
        {
            transform.position += Vector3.up * Time.deltaTime*10;
        }
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            obtain = true;
            GetComponent<Animator>().SetBool("obtained", true);
            AkSoundEngine.PostEvent("Load_game_play_sound", gameObject);
            message.GetComponent<Animator>().SetBool("MsgAnimBool", true);
        }
    }
}
