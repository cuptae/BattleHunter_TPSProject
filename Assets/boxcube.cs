using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxcube : MonoBehaviour
{
    private bool isPlayerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
            Debug.Log("Player is inside the box collider: " + isPlayerInside);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
            Debug.Log("Player is outside the box collider: " + isPlayerInside);
        }
    }

}
