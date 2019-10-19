using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public float maxTimerAttack = 1.0f;
    public float timerAttack = 0.0f;


    private FlockingManager flockingManager;
    private Collider wallCollider;

    private Transform player;    

    private void Start()
    {
        flockingManager = FlockingManager.instance;
        wallCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if(player != null)
        {
            timerAttack += Time.deltaTime;
            if(timerAttack >= maxTimerAttack)
            {
                flockingManager.GetRandomAttackAgent()?.AttackWall();
                timerAttack = 0.0f;
                maxTimerAttack = Random.Range(0.1f, 0.4f);
            }
        }
    }
    


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            player = collision.collider.transform;

            foreach (FlockingAgent agent in flockingManager.GetAgentsInCrew())
            {
                agent.SetAction(FlockingAgent.Action.destroyWall);
                agent.SetTargetWall(gameObject);
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            foreach (FlockingAgent agent in flockingManager.GetAgentsInCrew())
            {
                agent.SetAction(FlockingAgent.Action.followPlayer);
                agent.SetTargetWall(null);
                agent.SetCanBreakTheWall(false);
            }

            player = null;
        }
    }
}
