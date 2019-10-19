using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPath : MonoBehaviour
{


    public Transform[] nodes;

    private int currentIndex = 0;

    private NavMeshAgent navMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(navMeshAgent.remainingDistance == 0)
        {
            currentIndex++;
            if(currentIndex == nodes.Length)
            {
                currentIndex = 0;
            }
            navMeshAgent.SetDestination(nodes[currentIndex].position);
        }
    }
}
