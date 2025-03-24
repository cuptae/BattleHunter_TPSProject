using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public string jsonPath = Application.dataPath + "/2.Scripts/InGame/Player/CharacterData/Characterdata.json";
    private CharacterDataList characterDataList;
    void LoadCharacterData()
    {
        string jsonData = File.ReadAllText(jsonPath);
        characterDataList = JsonUtility.FromJson<CharacterDataList>(jsonData);
    }

    public CharacterData GetCharacterDataByName(string characterName)
    {
        return characterDataList.characterDataList.Find(c => c.characterName == characterName);
    }
}
