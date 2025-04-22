using UnityEngine;
using UnityEngine.UI;

public class UISoundSet : MonoBehaviour
{
    void Start()
    {
        AddUISoundToAll();
    }

    void AddUISoundToAll()
    {
        // 🔹 씬에 있는 모든 버튼 가져와서 UI 사운드 추가
        Button[] buttons = FindObjectsOfType<Button>(true);
        foreach (var button in buttons)
        {
            if(button.tag =="BackBtn")
            {
                button.onClick.AddListener(() => SoundManager.Instance.PlayUISound(UIType.BACK));
            }
            else
            {
                button.onClick.AddListener(() => SoundManager.Instance.PlayUISound(UIType.BUTTON));
            }
        }

        // 🔹 씬에 있는 모든 토글 가져와서 UI 사운드 추가
        Toggle[] toggles = FindObjectsOfType<Toggle>(true);
        foreach (var toggle in toggles)
        {
            toggle.onValueChanged.AddListener((_) => SoundManager.Instance.PlayUISound(UIType.TOGGLE));
        }

        Debug.Log("✅ 모든 UI 요소에 사운드 자동 추가 완료!");
    }
}
