using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{

    public int order;

    [TextArea(3, 10)]
    public string explanation;

    public AnimationCurve enterFadeIn = AnimationCurve.EaseInOut(0.0f, 0.2f, 0.6f, 1.0f);
    public AnimationCurve exitFadeOut = AnimationCurve.EaseInOut(0.0f, 0.2f, 0.6f, 1.0f);

    private float timer = 0.0f;
    private float speed = 0.8f;

    private enum States
    {
        onEnter,
        onUpdate,
        onExit
    }

    private States currentState = States.onEnter;

    private GameObject go;
    private RectTransform rec;
    private float delta = -370f;

    private void Awake()
    {
        TutorialManager.Instance.tutorials.Add(this);
        go = TutorialManager.Instance.hudInterface;
        rec = go.GetComponent<RectTransform>();
    }

    public virtual void CheckIfHappening() { }

    public void updateTutorial()
    {

        switch (currentState)
        {
            case States.onEnter:
                Vector2 pos;
                OnEnter();
                break;

            case States.onUpdate:
                CheckIfHappening();
                break;

            case States.onExit:
                pos = rec.anchoredPosition;
                pos.y = delta * (1 - exitFadeOut.Evaluate(timer));
                timer -= Time.deltaTime * speed;
                rec.anchoredPosition = pos;

                if (timer <= 0.0f)
                {
                    TutorialManager.Instance.SetNextTutorial(order + 1);
                }
                break;
        }

        
    }

    public virtual void OnEnter()
    {
        Vector2 pos = rec.anchoredPosition;
        pos.y = delta * (1 - enterFadeIn.Evaluate(timer));
        timer += Time.deltaTime * speed;
        rec.anchoredPosition = pos;

        if (timer >= 1.0f)
        {
            currentState = States.onUpdate;
            timer = 1.0f;
        }
    }

    public virtual void OnExit()
    {
        currentState = States.onExit;
    }
}
