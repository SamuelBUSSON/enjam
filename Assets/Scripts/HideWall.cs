using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HideWall : MonoBehaviour
{

    private Transform player;

    private Camera camera;
    
    private int layer_mask;

    private List<Transform> gameObjectHit;
    private Transform objectHit;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        camera = GetComponent<Camera>();

        gameObjectHit = new List<Transform>();

        layer_mask = LayerMask.GetMask("Default");
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, (player.position - transform.position), out hit, 100.0f, layer_mask))
        {
            objectHit = hit.transform;

            if (!objectHit.CompareTag("Player") && objectHit.GetComponent<MeshRenderer>())
            {
                AddObject(objectHit);
            }            
        }

        foreach (Transform objectTransform in gameObjectHit)
        {
            if (objectTransform.GetComponent<MeshRenderer>().material.color.a == 0 && objectTransform != objectHit)
            {
                objectTransform.GetComponent<MeshRenderer>().material.DOFade(1.0f, 0.8f).OnComplete(() => gameObjectHit.Remove(objectTransform));
            }
        }
    }

    public void AddObject(Transform objectToAdd)
    {
        if (!gameObjectHit.Contains(objectToAdd) && !objectToAdd.CompareTag("Floor"))
        {
            gameObjectHit.Add(objectHit);
            objectHit.GetComponent<MeshRenderer>().material.DOFade(0f, 0.8f);
        }
    }
}
