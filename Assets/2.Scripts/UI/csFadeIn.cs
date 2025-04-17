using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csFadeIn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("disable", 2.2f);
    }

    // Update is called once per frame
    void disable() 
    {
        gameObject.SetActive(false);
    }
}
