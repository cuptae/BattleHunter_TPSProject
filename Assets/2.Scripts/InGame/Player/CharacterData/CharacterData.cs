using System.Collections.Generic;

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
    public float damage;
}
[System.Serializable]
public class CharacterDataList
{
    public List<CharacterData> characterDataList;
}