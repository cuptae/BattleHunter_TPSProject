using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataReader : MonoSingleton<DataReader>
{
    public CharacterData data = new CharacterData(){characterName = "Gunner",maxHp = 100,dodgeTime = 0.7f,attackRange=10f,attackRate=0.5f,attackWalkSpeed = 3,walkSpeed = 7f,runSpeed=10f,rotationSpeed = 40f,damage = 30f};

}
