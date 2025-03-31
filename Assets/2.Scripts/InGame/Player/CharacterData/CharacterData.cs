using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using System.IO;
using UnityEngine;


public class CharacterStat
{
    //public string jsonPath = Application.dataPath + "/2.Scripts/InGame/Player/CharacterData/Characterdata.json";
    //public string jsonPath = Application.dataPath + "/Resources/Characterdata.json";
    public string jsonPath = Resources.Load<TextAsset>("Characterdata").text;
    private CharacterDataList characterDataList;
    CharacterData characterData;

    public int MaxHp => characterData.maxHp + modifyMaxHp;
    public int modifyMaxHp;

    public float DodgeTime => characterData.dodgeTime + modifyDodgeTime;
    public float modifyDodgeTime;

    public float AttackRange => characterData.attackRange + modifyAttackRange;
    public float modifyAttackRange;

    public float AttackRate => characterData.attackRate + modifyAttackRate;
    public float modifyAttackRate;

    public float AttackWalkSpeed => characterData.attackWalkSpeed + modifyAttackWalkSpeed;
    public float modifyAttackWalkSpeed;

    public float WalkSpeed => characterData.walkSpeed + modifyWalkSpeed;
    public float modifyWalkSpeed;

    public float RunSpeed => characterData.runSpeed + modifyRunSpeed;
    public float modifyRunSpeed;

    public float RotationSpeed => characterData.rotationSpeed + modifyRotationSpeed;
    public float modifyRotationSpeed;

    public int Damage => characterData.damage + modifyDamage;
    public int modifyDamage;

    public CharacterData GetCharacterDataByName(string characterName)
    {
        string jsonData = jsonPath;//File.ReadAllText(GameManager.Instance.jsonPath);
        characterDataList = JsonUtility.FromJson<CharacterDataList>(jsonData);
        characterData = characterDataList.characterDataList.Find(c => c.characterName == characterName);
        return characterData;
    }
    public void ModifyStat(string statType, float value)
    {
        switch (statType)
        {
            case "maxHp": modifyMaxHp += (int)value; break;
            case "dodgeTime": modifyDodgeTime += value; break;
            case "attackRange": modifyAttackRange += value; break;
            case "attackRate": modifyAttackRate += value; break;
            case "attackWalkSpeed": modifyAttackWalkSpeed += value; break;
            case "walkSpeed": modifyWalkSpeed += value; break;
            case "runSpeed": modifyRunSpeed += value; break;
            case "rotationSpeed": modifyRotationSpeed += value; break;
            case "damage": modifyDamage += (int)value; break;
            default: Debug.LogWarning($"Unknown stat: {statType}"); break;
        }
    }
}

[System.Serializable]
public class CharacterData
{
    public string characterName;
    public int maxHp;
    public float dodgeTime;
    public float attackRange;
    public float attackRate;
    public float attackWalkSpeed;
    public float walkSpeed;
    public float runSpeed;
    public float rotationSpeed;
    public int damage;
}
[System.Serializable]
public class CharacterDataList
{
    public List<CharacterData> characterDataList;
}