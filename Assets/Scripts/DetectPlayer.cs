using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DetectPlayer : MonoBehaviour
{

    public float minSoundRange = 3.0f;
    public float maxSoundRange = 5.0f;

    public float timerToGetCaptured = 3.0f;

    private float timer;

    private float currentSoundRange;
    private SphereCollider triggerZone;

    private Transform player;

    private List<FlockingAgent> agentInZone;

    private Animator anim;
    private float navMeshSpeed = 0.0f;

    private Transform firstAgent;
    

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
        currentSoundRange = Mathf.Lerp(minSoundRange, maxSoundRange, player.GetComponent<BoomBox>().GetPercentVolume());
        triggerZone.radius = currentSoundRange;

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FlockingAgent>())
        {
            GetComponentInParent<NavMeshAgent>().speed = 0;

            float angle = Vector3.Angle(transform.right, other.transform.right);

            transform.Rotate(new Vector3(0, -angle, 0));

            anim.SetTrigger("Detect");
            agentInZone.Add(other.GetComponent<FlockingAgent>());

            if(agentInZone.Count == 0)
            {
                firstAgent = other.transform;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<FlockingAgent>())
        {
            agentInZone.Remove(other.GetComponent<FlockingAgent>());           
            if(agentInZone.Count == 0)
            {
                GetComponentInParent<NavMeshAgent>().speed = navMeshSpeed;
            }

            if(other.transform == firstAgent)
            {
                firstAgent = null;
                timer = 0.0f;
            }
        }
    }
}
