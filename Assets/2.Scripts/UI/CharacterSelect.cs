using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    public Toggle warriorToggle;
    public Toggle gunnerToggle;
    public Toggle hackerToggle;

    public Transform portraitCamera;

    void Start()
    {
        // 각 토글이 변경될 때 SetCurCharacter() 실행
        warriorToggle.onValueChanged.AddListener(delegate { SetCurCharacter(); });
        gunnerToggle.onValueChanged.AddListener(delegate { SetCurCharacter(); });
        hackerToggle.onValueChanged.AddListener(delegate { SetCurCharacter(); });
    }

    void SetCurCharacter()
    {
        if (warriorToggle.isOn)
        {
            GameManager.Instance.curCharacter = Character.WARRIOR;
            portraitCamera.position = new Vector3(0,portraitCamera.position.y,portraitCamera.position.z);
        }
        else if (gunnerToggle.isOn)
        {
            GameManager.Instance.curCharacter = Character.GUNNER;
            portraitCamera.position = new Vector3(3,portraitCamera.position.y,portraitCamera.position.z);
        }
        else if (hackerToggle.isOn)
        {
            GameManager.Instance.curCharacter = Character.HACKER;
            portraitCamera.position = new Vector3(-3,portraitCamera.position.y,portraitCamera.position.z);
        }
        else
        {
            GameManager.Instance.curCharacter = Character.NONSELECTED;
        }
    }


}
