using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    public int skillLv = 1;
    Button button;


    void Awake()
    {
        button = GetComponent<Button>();
    }
    public void SkillLvUp()
    {
        if(skillLv <3)
        {
            skillLv++;
            if(skillLv == 3)
            {
                button.interactable = false;
            }
        }

    }
}
