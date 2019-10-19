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

    private bool onceWall = false;
    private GameObject wallTarget;
    private bool canBreakTheWall = false;
    private Vector3 basePosition;

    // Start is called before the first frame update
    void Start()
    {
        flockingManager = FlockingManager.instance;
        flockingManager.AddAgents(this);       

        navMeshAgent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player");

        basePosition = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInCrew)
        {
            switch (currentAction)
            {
                case Action.followPlayer:
                    GetComponent<Collider>().enabled = true;
                    Follow();
                    break;

                case Action.destroyWall:
                    DestroyWall();
                    GetComponent<Collider>().enabled = false;
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
        if (!onceWall)
        {
            navMeshAgent.SetDestination(GetPosNearPlayerForAttack());
            onceWall = true;
        }
        else
        {
            if(navMeshAgent.remainingDistance <= 0.1f)
            {
                basePosition = transform.position;
                canBreakTheWall = true;
            }
        }
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

    private Vector3 GetPosNearPlayerForAttack()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 posPlayer = player.transform.position;
        int numberOfAgentsInCrew = flockingManager.GetNumberOfAgentsAttack();
        float offset = 2.0f;

        return new Vector3(Random.Range(posPlayer.x - numberOfAgentsInCrew * offset, posPlayer.x + numberOfAgentsInCrew * offset), 0, Random.Range(posPlayer.z - numberOfAgentsInCrew * offset, posPlayer.z + numberOfAgentsInCrew * offset));
    }

    public void AttackWall()
    {
        Collider wallCollider = wallTarget.GetComponent<Collider>();
        
        float xPos = Random.Range(wallCollider.bounds.min.x, wallCollider.bounds.max.x);
        float yPos = Random.Range(transform.position.y, transform.position.y + 1.0f);
        float zPos = Random.Range(wallCollider.bounds.min.z, wallCollider.bounds.max.z);

        Sequence attackSequence = DOTween.Sequence();
        attackSequence.Append(transform.DOJump(new Vector3(xPos, yPos, zPos), 0.0f, 1, 0.3f));
        attackSequence.Append(transform.DOJump(basePosition, 2.0f, 1, 0.5f));

    }



    public void SetTargetWall(GameObject wall)
    {
        wallTarget = wall;
    }

    public bool CanBreakTheWall()
    {
        return canBreakTheWall;
    }

    public void SetCanBreakTheWall(bool canBreak)
    {
        canBreakTheWall = canBreak;
    }
}
