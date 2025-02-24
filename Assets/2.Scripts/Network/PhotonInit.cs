using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonInit : MonoBehaviour
{
    public string version = "ver 0.10";
    public PhotonLogLevel logLevel = PhotonLogLevel.Full;

    void Awake()
    {
        if(!PhotonNetwork.connected)
        {
            PhotonNetwork.ConnectUsingSettings(version);
            PhotonNetwork.logLevel = logLevel;
            PhotonNetwork.playerName = "GUEST " + Random.Range(1, 9999);
        }
    }

}
