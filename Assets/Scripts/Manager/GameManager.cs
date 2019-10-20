using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum State
    {
        play,
        victory,
        gameOver
    }

    public static GameManager instance;


    public State currentState;

    void Awake()
    {
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        else
        {
            //If instance already exists and it's not this:
            if (instance != this)
            {
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
            }
        }
        //Sets this to not be destroyed when reloading scene
      //  DontDestroyOnLoad(gameObject);        
    }

    // Start is called before the first frame update
    void Start()
    {
        currentState = State.play;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
