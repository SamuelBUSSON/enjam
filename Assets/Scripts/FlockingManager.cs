using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingManager : MonoBehaviour
{
    public static FlockingManager instance;

    public float playerSpeed = 1.0f;
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

    public List<FlockingAgent> GetAgents()
    {
        return agents;
    }
    public List<FlockingAgent> GetAgentsInCrew()
    {
        List<FlockingAgent> agentsInCrew = new List<FlockingAgent>();

        foreach (FlockingAgent agent in agents)
        {
            if (agent.IsInCrew())
            {
                agentsInCrew.Add(agent);
            }
        }

        return agentsInCrew;
    }

    public List<FlockingAgent> GetAttackAgents()
    {
        List<FlockingAgent> attackAgents = new List<FlockingAgent>();

        foreach (FlockingAgent agent in agents)
        {
            if (agent.CanBreakTheWall())
            {
                attackAgents.Add(agent);
            }
        }

        return attackAgents;
    }

    public void ResetDestination()
    {
        foreach (FlockingAgent agent in agents)
        {
            agent.SetDestinationReach(false);
        }
    }

    public FlockingAgent GetAgent(int index)
    {
        return agents[index];
    }

    public FlockingAgent GetRandomAgent()
    {
        return agents[Random.Range(0, agents.Count)];
    }

    public FlockingAgent GetRandomAttackAgent()
    {
        if (GetAttackAgents().Count > 0)
        {
            return GetAttackAgents()[Random.Range(0, GetAttackAgents().Count)];
        }
        else
        {
            return null;
        }
    }

    public FlockingAgent GetRandomAgentInCrew()
    {
        if(GetAgentsInCrew().Count > 0)
        {
            return GetAgentsInCrew()[Random.Range(0, GetAgentsInCrew().Count)];
        }
        else
        {
            return null;
        }
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

    public int GetNumberOfAgentsAttack()
    {
        return GetAttackAgents().Count;
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
