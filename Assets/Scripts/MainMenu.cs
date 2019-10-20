using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("SceneGiome");
    }

    public void PlayTutorial()
    {
        SceneManager.LoadScene("SceneTutorial");
    }

    public void goCredits()
    {

    }

    public void quitGame()
    {
        Application.Quit();
    }
}
