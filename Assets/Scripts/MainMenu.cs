using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void PlayGame()
    {
        AkSoundEngine.PostEvent("Load_game_play_sound", gameObject);
        SceneManager.LoadScene("SceneGiome");
    }

    public void PlayTutorial()
    {
        AkSoundEngine.PostEvent("Load_game_play_sound", gameObject);
        SceneManager.LoadScene("SceneTutorial");
    }

    public void goCredits()
    {
        AkSoundEngine.PostEvent("Load_credit_sound", gameObject);
    }

    public void quitGame()
    {
        AkSoundEngine.PostEvent("Load_credit_sound", gameObject);
        Application.Quit();
    }
}
