using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputId : MonoBehaviour
{
    public InputField inputField;
    public GameObject targetObject;

    void Awake()
    {
        inputField = GetComponent<InputField>();
    }
    void Start()
    {
        inputField.onEndEdit.AddListener(OnEndEdit);
    }

    void OnEndEdit(string text)
    {
        // 엔터 키 입력 확인
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            targetObject.SetActive(true);
            gameObject.SetActive(false); // 현재 오브젝트 비활성화
            PhotonNetwork.JoinLobby();
        }
    }
}
