using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Wall : MonoBehaviour
{

    [Header("Wall Life")]
    public int numberOfAgentsNeedToBreak = 5;
    public int health = 10;
    public float angleSpeed = 0.0f;

    [Header("Prisonner placement Zone")]
    public Transform prisonnerZone;

    private FlockingManager flockingManager;
    private Collider wallCollider;
    private Transform player;
    private int countAgent = 0;

    private bool deathEffectOnce = false;
    private Vector3 basePositionAngle;

    private void Start()
    {
        flockingManager = FlockingManager.instance;
        wallCollider = GetComponent<Collider>();
        basePositionAngle = new Vector3(transform.position.x, GetComponent<Collider>().bounds.min.y, transform.position.z);
    }

    private void Update()
    {
        if (player != null)
        {
            foreach (FlockingAgent agent in flockingManager.GetAgentsInCrew())
            {


                Collider prisonnerCollider = prisonnerZone.GetComponent<Collider>();

                float xPos = Random.Range(prisonnerCollider.bounds.min.x, prisonnerCollider.bounds.max.x);
                float zPos = Random.Range(prisonnerCollider.bounds.min.z, prisonnerCollider.bounds.max.z);

                agent.AttackWall(new Vector3(xPos, prisonnerZone.transform.position.y, zPos));
            }

        }

        if (health == 0)
        {
            Exit();
            if (transform.localEulerAngles.x > 270 || transform.localEulerAngles.x == 0.0f)
            {
                transform.RotateAround(basePositionAngle, Vector3.left, angleSpeed);
            }
            else
            {
                Sequence sequence = DOTween.Sequence();
                sequence.PrependInterval(1.0f);
                sequence.Append(transform.DOMoveY(transform.position.y - 1.0f, 1.0f)).OnComplete(() => Destroy(this));

            }
        }
    }

    public void IncrementCountAgent()
    {
        countAgent++;
    }

    public void DecreaseHealth()
    {
        if (health > 0)
        {
            health--;
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

                Collider prisonnerCollider = prisonnerZone.GetComponent<Collider>();

                float xPos = Random.Range(prisonnerCollider.bounds.min.x, prisonnerCollider.bounds.max.x);
                float zPos = Random.Range(prisonnerCollider.bounds.min.z, prisonnerCollider.bounds.max.z);

                while (!agent.SetAttackPosition(new Vector3(xPos, prisonnerZone.transform.position.y, zPos))) ;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Exit();
        }
    }

    private void Exit()
    {
        foreach (FlockingAgent agent in flockingManager.GetAgentsInCrew())
        {
            agent.SetAction(FlockingAgent.Action.followPlayer);
            agent.SetTargetWall(null);
            agent.SetCanBreakTheWall(false);
        }
        countAgent = 0;

        player = null;
    }
}
