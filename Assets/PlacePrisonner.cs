using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacePrisonner : MonoBehaviour
{

    public GameObject prisonner;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Instantiate(prisonner, transform.GetChild(i));
        }
    }
}
