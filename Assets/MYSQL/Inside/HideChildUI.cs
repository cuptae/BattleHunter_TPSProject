using UnityEngine;

public class HideChildUI : MonoBehaviour
{
    public GameObject targetUI; // ��Ȱ��ȭ�� �ڽ� ������Ʈ

    public void OnButtonClicked()
    {
        if (targetUI != null)
            targetUI.SetActive(false); // ��������
    }
}