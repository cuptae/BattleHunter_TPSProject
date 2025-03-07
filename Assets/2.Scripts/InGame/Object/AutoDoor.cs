using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    public bool isOpen = false;
    public Animation anim;

    private void Awake() {
        anim = GetComponent<Animation>();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"&& !isOpen)
        {
            Debug.Log("GateOpen");
            anim.Play("Open");
            isOpen = true;
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag == "Player" && isOpen)
        {
            Debug.Log("GateClose");
            anim.Play("Close");
            isOpen = false;
        }
    }

}
