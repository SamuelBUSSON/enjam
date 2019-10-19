using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;


public class FlockingAgent : MonoBehaviour
{

    public enum Action
    {
        destroyWall,
        followPlayer
    }


    public float size = 1.0f;     


    private FlockingManager flockingManager;
    private FlockingAgent neighborsSelected;
    private NavMeshAgent navMeshAgent;
    private Rigidbody rigidbody;

    private GameObject player;    

    private bool isInCrew = false;

    private bool isDestinationReach = false;
    private Action currentAction = Action.followPlayer;

    // Start is called before the first frame update
    void Start()
    {
        flockingManager = FlockingManager.instance;
        flockingManager.AddAgents(this);       

        navMeshAgent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (isInCrew)
        {
            switch (currentAction)
            {
                case Action.followPlayer:
                    Follow();
                    break;

                case Action.destroyWall:
                    DestroyWall();
                    break;
            }
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

           // Sequence searchSequence = DOTween.Sequence();
           // searchSequence.Append(transform.DOMove(neighborsSelected.transform.position + offsetNeighbors, 1.0f)).OnComplete(() => SetIsInCrew(true));
        }

        isInCrew = true;
    }    

    public void DestroyWall()
    {
        GetPosNearPlayer();
        
    }


    public void SetAction(Action type)
    {
        currentAction = type;
    }
    
    
    public FlockingAgent GetNeighbor()
    {
        return neighborsSelected;
    }
    

    public bool IsInCrew()
    {
        return isInCrew;
    }

    private void Follow()
    {
        if (Vector3.Distance(transform.position, player.transform.position) >= 5000.0f)
        {
            navMeshAgent.SetDestination(GetPosNearPlayer());
        }
        else
        {
            navMeshAgent.SetDestination(player.transform.position);
        }
    }

    public void SetDestinationReach(bool new_destination)
    {
        isDestinationReach = new_destination;
    }

    private Vector3 GetPosNearPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 posPlayer = player.transform.position;
        int numberOfAgentsInCrew = flockingManager.GetNumberOfAgentsInCrew();
        float offset = 2.0f;

        return new Vector3(Random.Range(posPlayer.x - numberOfAgentsInCrew * offset, posPlayer.x + numberOfAgentsInCrew * offset), 0, Random.Range(posPlayer.z - numberOfAgentsInCrew * offset, posPlayer.z + numberOfAgentsInCrew * offset));

    }

    public void SetAction()
    {

    }
}
