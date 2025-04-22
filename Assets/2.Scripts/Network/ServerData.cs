using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerData : MonoBehaviour
{
    [HideInInspector]
    public string serverName;

    public Text textServerName;
    // Start is called before the first frame update
    public void DisplayServerData()
    {
        textServerName.text = serverName;
    }
}
