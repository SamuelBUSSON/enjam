using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



public class PlayerMovement : MonoBehaviour
{

    public float speed = 1.0f;
    public float rangeRecruit = 5.0f;
    
    private Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Recruit();
    }

    
    private void Recruit()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0; i < FlockingManager.instance.GetNumberOfAgents(); i++)
            {
                FlockingAgent fa = FlockingManager.instance.GetAgent(i);

                if (!fa.IsInCrew())
                {
                    if (Vector3.Distance(fa.transform.position, transform.position) <= rangeRecruit)
                    {
                        fa.JoinCrew();
                    }
                }
            }
        }
    }

    private void Move()
    {
        Vector3 moveDirection = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z))
        {
            moveDirection.z += 1.0f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection.z -= 1.0f;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q))
        {
            moveDirection.x -= 1.0f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection.x += 1.0f;
        }

        moveDirection.Normalize();

        transform.DOMove(transform.position + moveDirection, 1/ speed);
        
    }
}
