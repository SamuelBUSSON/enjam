using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using cakeslice;

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

    private float maxHealth;

    private FlockingManager flockingManager;
    private Collider wallCollider;
    private Transform player;
    private int countAgent = 0;

    private bool deathEffectOnce = false;
    private Vector3 basePositionAngle;

    private Vector3 basePositon;
    private bool shakeComplete = true;

    private float angleCount;

    private float baseAngle;
    private bool over = false;

    private bool shakeTheWall = false;

    private bool once = false;

    uint punchToWallId;

    private void Start()
    {
        flockingManager = FlockingManager.instance;
        wallCollider = GetComponent<Collider>();
        basePositionAngle = new Vector3(transform.position.x, GetComponent<Collider>().bounds.min.y, transform.position.z);        

        baseAngle = transform.eulerAngles.y;

        basePositon = new Vector3();
        basePositon = transform.position;

        maxHealth = health;

        numberOfAgentsNeedToBreak--;


    }

    private void Update()
    {
        if (!over)
        {
            if (player != null)
            {

                if (!once)
                {
                    punchToWallId = AkSoundEngine.PostEvent("Punch_to_wall", gameObject);

                    once = true;
                }

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

            if (health == 0)
            {              

                AkSoundEngine.StopPlayingID(punchToWallId);

                once = false;

                player = null;
                Exit();
                GetComponent<Collider>().enabled = false;
                angleCount += angleSpeed;              
                
                if (angleCount <= 90f && angleCount >= -90f)
                {
                    if (baseAngle != 0)
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
                    AkSoundEngine.PostEvent("Wall_destruction", gameObject);
                    over = true;
                }
            }

            if (shakeTheWall && health == maxHealth)
            {
                if (GetComponent<Outline>())
                {
                    GetComponent<Outline>().enabled = true;
                }

                if (shakeComplete)
                {
                    shakeComplete = false;                    

                    Sequence moveSequence = DOTween.Sequence();
                    moveSequence.Append(transform.DOJump(basePositon, 0.5f, 3, 0.8f)).OnComplete(() => shakeComplete = true);
                }
            }
            else
            {
                if (GetComponent<Outline>())
                {
                    GetComponent<Outline>().enabled = false;
                }
            }
        }
    }

    public void SetShakeTheWall(bool new_shake)
    {
        if(flockingManager.GetNumberOfAgentsInCrew() >= numberOfAgentsNeedToBreak)
        {
            shakeTheWall = new_shake;
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
