using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoSingleton<GameManager>
{
    public Character curCharacter = Character.NONSELECTED;

    //public string jsonPath = Application.dataPath + "/2.Scripts/InGame/Player/CharacterData/Characterdata.json";
    //private CharacterDataList characterDataList;
    // void Start()
    // {
    //  LoadCharacterData();   
    // }
    // void LoadCharacterData()
    // {
    //     string jsonData = File.ReadAllText(jsonPath);
    //     characterDataList = JsonUtility.FromJson<CharacterDataList>(jsonData);
    // }

    // public CharacterData GetCharacterDataByName(string characterName)
    // {
    //     return characterDataList.characterDataList.Find(c => c.characterName == characterName);
    // }
}
