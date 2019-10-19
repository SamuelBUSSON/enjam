using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;
using Cinemachine;

public class FlockingAgent : MonoBehaviour
{

    public enum Action
    {
        destroyWall,
        followPlayer
    }

    public float size = 1.0f;

    public GameObject hitParticleEffect;

    private FlockingManager flockingManager;
    private FlockingAgent neighborsSelected;
    private NavMeshAgent navMeshAgent;
    private Rigidbody rigidbody;

    private CinemachineVirtualCamera vcam;
    private CinemachineBasicMultiChannelPerlin noise;

    private Animator animator;

    private GameObject player;    

    private bool isInCrew = false;

    private bool isDestinationReach = false;
    private Action currentAction = Action.followPlayer;

    private bool onceWall = false;
    private GameObject wallTarget;
    private bool canBreakTheWall = false;
    private Vector3 basePositionAttack;
    private bool attackOnce = false;

    private bool onceIncrement = false;

    // Start is called before the first frame update
    void Start()
    {
        flockingManager = FlockingManager.instance;
        flockingManager.AddAgents(this);       

        navMeshAgent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player");

        basePositionAttack = new Vector3();

        animator = GetComponentInChildren<Animator>();        

        vcam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
        noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
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
                    ResetCarac();
                    break;

                case Action.destroyWall:
                    DestroyWall();
                    break;
            }            
        }
    }

    public void ResetCarac()
    {
        onceIncrement = false;
        Noise(0.0f, 0.0f);
        onceWall = false;
        canBreakTheWall = false;
        attackOnce = false;
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
        }
        isInCrew = true;

        animator.SetBool("IsIdle", false);
        animator.SetBool("IsDancing", true);
    }    

    public void DestroyWall()
    {
        if (!onceWall)
        {
            navMeshAgent.SetDestination(basePositionAttack);
            onceWall = true;
        }
        else
        {
            if(navMeshAgent.remainingDistance <= 1.5f)
            {
                if (!onceIncrement)
                {
                    canBreakTheWall = true;
                    wallTarget.GetComponent<Wall>().IncrementCountAgent();
                    onceIncrement = true;
                }
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

    public void AttackWall(Vector3 returnPos)
    {
        if (!attackOnce)
        {
            attackOnce = true;

            Collider wallCollider = wallTarget.GetComponent<Collider>();

            Vector3 heading = new Vector3(wallTarget.transform.position.x, 0, wallTarget.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z); 
            float distance = -heading.magnitude;
            Vector3 direction = heading / distance;

            float xPos = Random.Range(wallCollider.bounds.min.x, wallCollider.bounds.max.x);
            float yPos = Random.Range(transform.position.y, transform.position.y + 1.0f);
            float zPos = Random.Range(wallCollider.bounds.min.z, wallCollider.bounds.max.z);

            Sequence attackSequence = DOTween.Sequence();
            attackSequence.Append(transform.DOJump(new Vector3(xPos, yPos, zPos) + direction, 0.0f, 1, 0.3f)).OnStart(() => Noise(5.0f, 1.0f));
            InstantiateEffect(hitParticleEffect, new Vector3(xPos, yPos, zPos) + direction);
            attackSequence.Append(transform.DOJump(returnPos, 2.0f, 1, 0.5f)).OnComplete(() => Noise(0.0f, 0.0f)).OnComplete(() => attackOnce = false);

            wallTarget.GetComponent<Wall>().DecreaseHealth();
        }
    }

    public void InstantiateEffect(GameObject effect, Vector3 position)
    {
       Destroy(Instantiate(effect, position, Quaternion.identity), effect.GetComponent<ParticleSystem>().main.duration);
    }

    public bool SetAttackPosition(Vector3 new_pos)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(new_pos, out hit, 1.0f, NavMesh.AllAreas))
        {
            basePositionAttack = hit.position;
            return true;
        }
        return false;
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

    public void Noise(float amplitudeGain, float frequencyGain)
    {
        noise.m_AmplitudeGain = amplitudeGain;
        noise.m_FrequencyGain = frequencyGain;
    }
}
