using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldRequireLength : MonoBehaviour
{
    public InputField inputField;
    public Button doneButton;
    public Sprite activeImage;
    public Sprite InactiveImage;
    public Color activeColor;
    public Color InactiveColor;
    private Text doneButtonText;

    public int requiredLength = 2;
    // Start is called before the first frame update
    void Start()
    {
        doneButtonText = doneButton.GetComponentInChildren<Text>();
        doneButton.interactable = false;
        inputField.onValueChanged.AddListener(OnInputValueChanged);
    }

    

    void OnInputValueChanged(string value)
    {
        // 인풋필드의 값이 변경될 때 호출되는 메서드
        // 입력 값의 길이가 필요한 길이와 같다면 Done 버튼을 활성화, 아니면 비활성화
        if (value.Length >= requiredLength)
        {
            doneButton.interactable = true;
            doneButton.image.sprite = activeImage;
            doneButtonText.color = activeColor;
        }
        else
        {
            doneButton.interactable = false;
            doneButton.image.sprite = InactiveImage;
            doneButtonText.color = InactiveColor;
        }
    }
    // Update is called once per frame

}
