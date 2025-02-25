using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.RestService;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class PhotonLobby : MonoBehaviour
{
    public string version = "Ver 0.10";
    public PhotonLogLevel logLevel = PhotonLogLevel.Informational;
    public Text userId;


    void Awake()
    {
        if(!PhotonNetwork.connected)
        {
            PhotonNetwork.ConnectUsingSettings(version);
            Debug.Log("connected!");
            PhotonNetwork.playerName = "USER"+Random.Range(0,9999);
        }
    }

    void OnJoinedLobby()
    {
        Debug.Log("Join Lobby!!!");
        userId.text = GetUserID();
    }

    string GetUserID()
    {
        string userId = PlayerPrefs.GetString("USER_ID");
        if(userId.IsNullOrEmpty())
        {
            userId = "USER"+Random.Range(0,1000);
        }
        return userId;
    }
}
