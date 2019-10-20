using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using AK.Wwise;

public class PlayerMovement : MonoBehaviour
{
   
    public float rangeRecruit = 5.0f;
    public float speed = 500;

    private FlockingManager flockingManager;
    private Vector3 position;
    private Rigidbody rigidbody;

    private NavMeshAgent navMeshAgent;
    private Vector3 forwardVector;
    

    // Start is called before the first frame update
    void Start()
    {
        flockingManager = FlockingManager.instance;

        position = transform.position;
        rigidbody = GetComponent<Rigidbody>();

        navMeshAgent = GetComponent<NavMeshAgent>();
        forwardVector = new Vector3(0,0,0);
        forwardVector = transform.forward;
        
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

        if (Input.anyKey && rigidbody.velocity.y >= -0.1f)
        {

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

            float angle = Angle(forwardVector, moveDirection);

            if ( (moveDirection.x > 0.0 || moveDirection.y > 0.0 || moveDirection.z > 0.0)  && !(moveDirection.z < 0.0 && moveDirection.x > 0.0))
            {
                angle = -angle;
            }

            transform.DOLocalRotate(new Vector3(transform.eulerAngles.x, angle, transform.eulerAngles.z), 0.3f);
                

            rigidbody.velocity = moveDirection * flockingManager.playerSpeed;
        }
        else
        {
            rigidbody.velocity = new Vector3(0, rigidbody.velocity.y, 0);
        }
    }

    public float Angle(Vector3 from, Vector3 to)
    {
        return Mathf.Acos(Mathf.Clamp(Vector3.Dot(from.normalized, to.normalized), -1f, 1f)) * 57.29578f;
    }
}

