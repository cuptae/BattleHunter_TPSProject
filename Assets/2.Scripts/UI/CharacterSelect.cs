using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    public Toggle warriorToggle;
    public Toggle gunnerToggle;
    public Toggle hunterToggle;

    void Start()
    {
        // 각 토글이 변경될 때 SetCurCharacter() 실행
        warriorToggle.onValueChanged.AddListener(delegate { SetCurCharacter(); });
        gunnerToggle.onValueChanged.AddListener(delegate { SetCurCharacter(); });
        hunterToggle.onValueChanged.AddListener(delegate { SetCurCharacter(); });
    }

    void SetCurCharacter()
    {
        if (warriorToggle.isOn)
        {
            GameManager.Instance.curCharacter = Character.WARRIOR;
        }
        else if (gunnerToggle.isOn)
        {
            GameManager.Instance.curCharacter = Character.GUNNER;
        }
        else if (hunterToggle.isOn)
        {
            GameManager.Instance.curCharacter = Character.HUNTER;
        }
        else
        {
            GameManager.Instance.curCharacter = Character.NONSELECTED;
        }
    }
}
