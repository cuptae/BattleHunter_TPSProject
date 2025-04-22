using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intercom : MonoBehaviour
{
    public Animator[] door;

    public void OpenDoor()
    {
        foreach (var anim in door)
        {
            anim.SetTrigger("Open");
        }
    }
}
