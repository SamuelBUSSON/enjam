using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TutorialManager : MonoBehaviour
{

    public List<Tutorial> tutorials = new List<Tutorial>();
    public TextMeshProUGUI expText;
    public GameObject hudInterface;

    private static TutorialManager instance;
    private Tutorial currentTutorial;

    //Singleton
    public static TutorialManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<TutorialManager>();
            }
            if(instance == null)
            {
                Debug.Log("There is no Tutorial Manager");
            }
            return instance;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        SetNextTutorial(0);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            CompletedTutorial();
        }


        if (currentTutorial)
        {
            currentTutorial.updateTutorial();
        }
    }


    public void SetNextTutorial(int currentOrder)
    {
        currentTutorial = GetTutorialByOrder(currentOrder);

        if (!currentTutorial)
        {
            CompletedAllTutorials();
            return;
        }

        expText.text = currentTutorial.explanation;
    }

    public void CompletedTutorial()
    {
        if (currentTutorial)
        {
            currentTutorial.OnExit();
        }
    }

    public void CompletedAllTutorials()
    {
        AkSoundEngine.StopAll();
        expText.text = "You have completed all tutorials";
        SceneManager.LoadScene("Menu");

    }

    public Tutorial GetTutorialByOrder(int Order)
    {
        for(int i = 0; i < tutorials.Count; i++)
        {
            if(tutorials[i].order == Order)
            {
                return tutorials[i];
            }
        }
        return null;
    }
}
