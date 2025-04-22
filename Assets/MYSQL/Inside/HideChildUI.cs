using UnityEngine;

public class HideChildUI : MonoBehaviour
{
    public GameObject targetUI; // 비활성화할 자식 오브젝트

    public void OnButtonClicked()
    {
        if (targetUI != null)
            targetUI.SetActive(false); // 꺼버리기
    }
}