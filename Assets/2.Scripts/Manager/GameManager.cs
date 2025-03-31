using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoSingleton<GameManager>
{
    public Character curCharacter = Character.NONSELECTED;
    public string jsonPath;


    protected override void Awake()
    {
        base.Awake();
        jsonPath =Resources.Load<TextAsset>("Characterdata").text;
        //jsonPath = Application.dataPath + "/Resources/Characterdata.json";
    }

    void Start()
    {
        Debug.Log(jsonPath);   
    }
}
