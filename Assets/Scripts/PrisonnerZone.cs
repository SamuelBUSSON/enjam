using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrisonnerZone : MonoBehaviour
{

    public Wall attachWall;
    public bool reverseAngle = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && attachWall.health == attachWall.GetMaxHealth())
        {
            if (reverseAngle)
            {
                attachWall.reverseAngle = true;
            }
            else
            {
                attachWall.reverseAngle = false;
            }
            attachWall.SetClosestZone(transform);
        }
    }
}
