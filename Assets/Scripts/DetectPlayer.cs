using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DetectPlayer : MonoBehaviour
{

    public float minSoundRange = 3.0f;
    public float maxSoundRange = 5.0f;

    public float timerToGetCaptured = 3.0f;

    public Light spotLigh;

    private float timer;
    private float playerTimer;

    private float currentSoundRange;
    private SphereCollider triggerZone;

    private Transform player;

    private List<FlockingAgent> agentInZone;

    private Animator anim;
    private float navMeshSpeed = 0.0f;

    private Transform firstAgent;

    private bool playerIsIn = false;

    // Start is called before the first frame update
    void Start()
    {
        triggerZone = GetComponent<SphereCollider>();
        anim = GetComponent<Animator>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        agentInZone = new List<FlockingAgent>();

        navMeshSpeed = GetComponentInParent<NavMeshAgent>().speed;
    }

    // Update is called once per frame
    void Update()
    {

        currentSoundRange = Mathf.Lerp(minSoundRange, maxSoundRange, player.GetComponentInChildren<BoomBox>().GetPercentVolume());
        triggerZone.radius = currentSoundRange;
        spotLigh.spotAngle = Mathf.Lerp(25, 120, player.GetComponentInChildren<BoomBox>().GetPercentVolume());
        

        if (firstAgent)
        {
            timer += Time.deltaTime;
            if(timer >= timerToGetCaptured)
            {
                foreach (FlockingAgent agent in agentInZone)
                {
                    agent.SetAction(FlockingAgent.Action.captured);
                }
                timer = 0.0f;
            }
        }

        if (playerIsIn)
        {
            playerTimer += Time.deltaTime;
            if (playerTimer >= timerToGetCaptured)
            {
                AkSoundEngine.PostEvent("Game_over", gameObject);
                playerTimer = 0.0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FlockingAgent>())
        {            
            DetectEntity(other);

           

            if (agentInZone.Count == 0)
            {
                firstAgent = other.transform;
            }
            
            agentInZone.Add(other.GetComponent<FlockingAgent>());
        }

        if (other.CompareTag("Player"))
        {
            DetectEntity(other);
            player = other.transform;
            playerIsIn = true;
        }
    }

    private void DetectEntity(Collider other)
    {
        AkSoundEngine.PostEvent("Guardian_interpelation_voice", gameObject);

        float angle = Vector3.Angle(transform.right, other.transform.right);
        transform.Rotate(new Vector3(0, -angle, 0));

        GetComponentInParent<NavMeshAgent>().speed = 0;
        anim.SetBool("Detect", true);
        anim.SetBool("IsWalking", false);
    }

    private void EntityMoveAway(Collider other)
    {
        if (agentInZone.Count == 0)
        {
            GetComponentInParent<NavMeshAgent>().speed = navMeshSpeed;
            anim.SetBool("Detect", false);
            anim.SetBool("IsWalking", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<FlockingAgent>())
        {
            agentInZone.Remove(other.GetComponent<FlockingAgent>());           


            if(other.transform == firstAgent)
            {
                firstAgent = null;
                timer = 0.0f;
            }
            EntityMoveAway(other);
        }


        if (other.CompareTag("Player"))
        {
            EntityMoveAway(other);
            playerIsIn = false;
        }
    }
}
