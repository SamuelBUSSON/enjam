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
    public Transform prisonnerZone2;

    [HideInInspector()]
    public bool reverseAngle = false;

    private Transform closestPrisonnerZone;

    private FlockingManager flockingManager;
    private Collider wallCollider;
    private Transform player;
    private int countAgent = 0;

    private bool deathEffectOnce = false;
    private Vector3 basePositionAngle;

    private float angleCount;

    private float baseAngle;
    private bool over = false;


    private void Start()
    {
        flockingManager = FlockingManager.instance;
        wallCollider = GetComponent<Collider>();
        basePositionAngle = new Vector3((GetComponent<Collider>().bounds.min.x + GetComponent<Collider>().bounds.max.x) /2, GetComponent<Collider>().bounds.min.y, (GetComponent<Collider>().bounds.min.z + GetComponent<Collider>().bounds.max.z) / 2);

        baseAngle = transform.eulerAngles.y;
    }

    private void Update()
    {
        if (player != null)
        {
            if (flockingManager.GetNumberOfAgentsInCrew() >= numberOfAgentsNeedToBreak)
            {
                foreach (FlockingAgent agent in flockingManager.GetAgentsInCrew())
                {
                    agent.SetAction(FlockingAgent.Action.destroyWall);
                    agent.SetTargetWall(gameObject);

                    Collider prisonnerCollider = closestPrisonnerZone.GetComponent<Collider>();

                    float xPos = Random.Range(prisonnerCollider.bounds.min.x, prisonnerCollider.bounds.max.x);
                    float zPos = Random.Range(prisonnerCollider.bounds.min.z, prisonnerCollider.bounds.max.z);

                    agent.AttackWall(new Vector3(xPos, closestPrisonnerZone.transform.position.y, zPos));                    
                }
            }
        }

        if (health == 0 && !over)
        {
            player = null;
            Exit();
            GetComponent<Collider>().enabled = false;
            angleCount += angleSpeed;
            Debug.Log(angleCount);
            if (angleCount <= 90f && angleCount >= -90f)
            {
                if(baseAngle != 0)
                {
                    transform.RotateAround(basePositionAngle, Quaternion.AngleAxis(baseAngle, Vector3.up) * Vector3.left, reverseAngle ? -angleSpeed : angleSpeed);
                }
                else
                {
                    transform.RotateAround(basePositionAngle, Vector3.left, reverseAngle ? -angleSpeed : angleSpeed);
                }
            }
            else
            {
                over = true;
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
        }
    }

    public void SetClosestZone(Transform t)
    {
        closestPrisonnerZone = t;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            player = null;
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
        
    }
}
