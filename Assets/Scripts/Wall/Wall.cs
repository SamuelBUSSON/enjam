using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public GameObject wallObject;

    private FlockingManager flockingManager;
    private Collider wallCollider;

    private void Start()
    {
        flockingManager = FlockingManager.instance;
        wallCollider = GetComponent<Collider>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (FlockingAgent agent in flockingManager.GetAgentsInCrew())
            {
                agent.SetAction(FlockingAgent.Action.destroyWall);
            }
        }

        Random.Range(wallCollider.bounds.min.x, wallCollider.bounds.max.x);
        Random.Range(wallCollider.bounds.min.z, wallCollider.bounds.max.z);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Out");
        }
    }
}
