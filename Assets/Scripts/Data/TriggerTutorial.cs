using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTutorial : Tutorial
{

    private bool isCurrentTutorial = false;

    public GameObject objecToShow;

    public Canvas hitArrow;

    public override void CheckIfHappening()
    {
        if (objecToShow)
        {
            objecToShow.SetActive(true);
        }

        isCurrentTutorial = true;
        hitArrow.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isCurrentTutorial)
        {
            return;
        }
        TutorialManager.Instance.CompletedTutorial();
        isCurrentTutorial = false;
        hitArrow.enabled = false;


    }
}
