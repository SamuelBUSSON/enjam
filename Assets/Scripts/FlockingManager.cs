using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    public static FlockingManager instance;

    public float speed = 1.0f;
    public float offsetLength = 1.5f;

    private Vector3 flockCenter;
    private Vector3 flockVelocity;

    private List<FlockingAgent> agents;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        else
        {
            //If instance already exists and it's not this:
            if (instance != this)
            {
                //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
                Destroy(gameObject);
            }
        }
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        agents = new List<FlockingAgent>();
    }
    

    // Update is called once per frame
    void Update()
    {
        Vector3 theCenter = Vector3.zero;
        
        foreach (FlockingAgent boid in agents)
        {
            theCenter = theCenter + boid.transform.localPosition;
        }

        flockCenter = theCenter / (agents.Count);
    }

    public List<FlockingAgent> GetAgents()
    {
        return agents;
    }

    public FlockingAgent GetAgent(int index)
    {
        return agents[index];
    }

    public FlockingAgent GetRandomAgent()
    {
        return agents[Random.Range(0, agents.Count)];
    }

    public FlockingAgent GetRandomAgentInCrew()
    {
        FlockingAgent fa = agents[Random.Range(0, agents.Count)];

        while (!fa.IsInCrew())
        {
            fa = agents[Random.Range(0, agents.Count)];
        }

        return fa;
    }

    public void AddAgents(FlockingAgent agentToAdd)
    {
        for (int i = 0; i < agents.Count; i++)
        {
            Physics.IgnoreCollision(agentToAdd.GetComponent<Collider>(), agents[i].GetComponent<Collider>());
        }
        agents.Add(agentToAdd);
    }

    public bool CheckPosition(Vector3 position)
    {
        for (int i = 0; i < agents.Count; i++)
        {
            if(Vector3.Distance(position, agents[i].transform.position) <= 1.5f)
            {
                return false;
            }
        }

        return true;
    }

    public int GetNumberOfAgents()
    {
        return agents.Count;
    }

    public int GetNumberOfAgentsNotInCrew()
    {
        int count = 0;

        foreach (FlockingAgent boid in agents)
        {
            if (!boid.IsInCrew())
            {
                count++;
            }
        }
        return count;
    }

    public int GetNumberOfAgentsInCrew()
    {
        return agents.Count - GetNumberOfAgentsNotInCrew();
    }
}
