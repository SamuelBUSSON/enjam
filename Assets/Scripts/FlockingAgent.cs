using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlockingAgent : MonoBehaviour
{

    public float rangeRecruit = 5.0f;

    public float size = 1.0f;

    public bool isPlayer = false;

    private FlockingManager flockingManager;
    private FlockingAgent neighborsSelected;
    private Vector3 offsetNeighbors;

    private float timerJump;
    private float maxTimerJump;

    private bool isInCrew = false;

    // Start is called before the first frame update
    void Start()
    {
        flockingManager = FlockingManager.instance;
        flockingManager.AddAgents(this);
       
        if (isPlayer)
        {
            isInCrew = true;
        }
        else
        {
            maxTimerJump = Random.Range(0.5f, 2.5f);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (isInCrew)
        {
            if (isPlayer)
            {
                Recruit();
            }
            Move();
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Sequence searchSequence = DOTween.Sequence();
            searchSequence.Append(transform.DOJump(transform.position, 3.0f, 1, 1.0f));
        }
    }

    public void JoinCrew()
    {
        if (flockingManager.GetNumberOfAgentsInCrew() > 0)
        {
            FlockingAgent neighFa = flockingManager.GetRandomAgentInCrew();
            while (neighFa.GetNeighbor() == this)
            {
                neighFa = flockingManager.GetRandomAgentInCrew();
            }

            neighborsSelected = neighFa;

            while(!flockingManager.CheckPosition(neighborsSelected.transform.position + offsetNeighbors))
            {
                offsetNeighbors = new Vector3(Random.Range(-flockingManager.offsetLength, flockingManager.offsetLength), 0, Random.Range(-flockingManager.offsetLength, flockingManager.offsetLength));
            }

            Sequence searchSequence = DOTween.Sequence();
            searchSequence.Append(transform.DOMove(neighborsSelected.transform.position + offsetNeighbors, 1.0f)).OnComplete(() => SetIsInCrew(true));
        }
    }
    
    public void SetIsInCrew(bool crew)
    {
        isInCrew = crew;
    }

    public IEnumerator JoinCrewMove()
    {
        float time = 1.0f;

        transform.DOMove(neighborsSelected.transform.position + offsetNeighbors, time);
        yield return new WaitForSeconds(time);
    }
    
    public FlockingAgent GetNeighbor()
    {
        return neighborsSelected;
    }

    private void Recruit()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < FlockingManager.instance.GetNumberOfAgents(); i++)
            {
                FlockingAgent fa = FlockingManager.instance.GetAgent(i);

                if (!fa.IsInCrew())
                {
                    if (Vector3.Distance(fa.transform.position, transform.position) <= rangeRecruit)
                    {
                        fa.JoinCrew();
                    }
                }
            }
        }
    }

    private void Move()
    {
        Vector3 moveDirection = new Vector3(0, 0, 0);

        if (Input.anyKey)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z))
            {
                moveDirection.z += 1.0f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                moveDirection.z -= 1.0f;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q))
            {
                moveDirection.x -= 1.0f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                moveDirection.x += 1.0f;
            }

            moveDirection.Normalize();

            transform.DOMove(transform.position + moveDirection, 1 / flockingManager.speed);
        }
    }

    public bool IsInCrew()
    {
        return isInCrew;
    }


    /*
    public void Separation()
    {
        Vector3 separationForce = new Vector3(0, 0, 0);

        for (int i = 0; i < flockingManager.GetAgents().Count; i++)
        {

            if(flockingManager.GetAgent(i) != this)
            {
                Vector3 toAgent = transform.position - flockingManager.GetAgent(i).transform.position;
                separationForce += toAgent.normalized / toAgent.magnitude;
            }
        }

        transform.position += separationForce / flockingManager.separationPower;
    }

    public void Cohesion()
    {
        Vector3 cohesionForce = new Vector3(0, 0, 0);
        Vector3 centerOfMass = new Vector3(0, 0, 0);

        int neighborCount = 0;

        for (int i = 0; i < flockingManager.GetAgents().Count; i++)
        {
            if (flockingManager.GetAgent(i) != this)
            {
                centerOfMass += flockingManager.GetAgent(i).transform.position;
                neighborCount++;
            }
        }

        if(neighborCount > 0)
        {
            centerOfMass /= neighborCount;
        }

        transform.position += centerOfMass;
    }*/
}
