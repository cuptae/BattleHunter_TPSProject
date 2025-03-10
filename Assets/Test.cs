using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public bool isInteractive;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            isInteractive = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            isInteractive = false;
        }
    }
}
