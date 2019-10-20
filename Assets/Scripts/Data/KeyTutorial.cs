using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTutorial : Tutorial
{

    public List<KeyCode> Keys = new List<KeyCode>();    

    private bool containsAxis = false;

    public List<int> axisValueH;
    public List<int> axisValueV;


    public void Start()
    {

    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {

        base.OnExit();
    }

    public override void CheckIfHappening()
    {

        for (int i = 0; i < Keys.Count; i++)
        {
            if (Input.GetKeyDown(Keys[i]))
            {
                Keys.RemoveAt(i);
                break;
            }

            if (containsAxis)
            {
                // Getting the inputs
                var h = Input.GetAxisRaw("Horizontal");
                var v = Input.GetAxisRaw("Vertical");
                
                for(int j = 0; j < axisValueH.Count; j++)
                {
                    if(h == axisValueH[j])
                    {
                        axisValueH.RemoveAt(j);
                        break;
                    }
                }

                for (int j = 0; j < axisValueV.Count; j++)
                {
                    if (v == axisValueV[j])
                    {
                        axisValueV.RemoveAt(j);
                        break;
                    }
                }

                if (axisValueV.Count == 0 && axisValueH.Count == 0)
                {
                    TutorialManager.Instance.CompletedTutorial();
                }
            }



        }
        if(Keys.Count == 0)
        {
            TutorialManager.Instance.CompletedTutorial();
        }
    }
}
