using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject optionPanel;  // 옵션 창
    public GameObject crossHair;    // 크로스헤어 UI
    private bool isOptionOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleOption();
        }
    }

    public void ToggleOption()
    {
        isOptionOpen = !isOptionOpen;
        optionPanel.SetActive(isOptionOpen);
        crossHair.SetActive(!isOptionOpen);

        if (isOptionOpen)
        {
            Cursor.lockState = CursorLockMode.None; // 마우스 활성화
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked; // 마우스 숨김 (게임 모드)
            Cursor.visible = false;
        }
    }
}