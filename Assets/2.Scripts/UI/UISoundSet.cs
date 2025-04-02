using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIAudioHandler : MonoBehaviour
{
    [SerializeField] private UIType uiSoundType; // 🎵 UI 사운드 타입 지정 (Inspector에서 설정)

    private void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => PlaySound(uiSoundType)); // 클릭 시 사운드
            AddPointerEnterSound(UIType.CROSSBTN); // 마우스 오버 시 사운드 추가
        }

        Toggle toggle = GetComponent<Toggle>();
        if (toggle != null)
        {
            toggle.onValueChanged.AddListener((isOn) => PlaySound(uiSoundType)); // 토글 변경 시 사운드
            AddPointerEnterSound(UIType.CROSSBTN); // 마우스 오버 시 사운드 추가
        }
    }

    // 🎵 사운드 재생 함수
    private void PlaySound(UIType type)
    {
        SoundManager.Instance.PlayUISound(type);
    }

    // 🔊 마우스 오버 시 소리 추가
    private void AddPointerEnterSound(UIType hoverSound)
    {
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => PlaySound(hoverSound));
        trigger.triggers.Add(entry);
    }
}
