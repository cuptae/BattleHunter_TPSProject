using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerUploader : MonoBehaviour
{
    [Header("UI ����")]
    public InputField playerInputField;     // �÷��̾� �̸� �Է� �ʵ�
    public Button uploadButton;             // ���ε� ��ư

    [Header("���� ���� ����")]
    public JobManager jobManager;           // ĳ���� ID �޾ƿ���
    public SkillSlotManager slotManager;    // Q, E ���Կ��� ��ų ID �޾ƿ���

    void Start()
    {
        uploadButton.onClick.AddListener(OnUploadClicked);
    }

    void OnUploadClicked()
    {
        string playerName = playerInputField.text;
        int characterId = jobManager.GetSelectedCharacterId();

        // ���� �Ǵ� null�̸� default_player�� ����
        if (string.IsNullOrWhiteSpace(playerName))
        {
            playerName = "default_player";
        }

        if (characterId == -1)
        {
            Debug.LogWarning("ĳ���͸� �������ּ���.");
            return;
        }

        List<int> skills = slotManager.GetSelectedSkillIds();

        int skillQ = skills.Count > 0 ? skills[0] : -1;
        int skillE = skills.Count > 1 ? skills[1] : -1;

        Debug.Log($"������ ������ �� �̸�: {playerName}, ĳ���� ID: {characterId}, Q: {skillQ}, E: {skillE}");
        StartCoroutine(SendToServer(playerName, characterId, skillQ, skillE));
    }

    IEnumerator SendToServer(string player, int characterId, int skillQ, int skillE)
    {
        WWWForm form = new WWWForm();
        form.AddField("player", player);
        form.AddField("character_id", characterId);
        form.AddField("skill_q", skillQ);
        form.AddField("skill_e", skillE);

        UnityWebRequest www = UnityWebRequest.Post("http://192.168.0.24:8080/insert_player.php", form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("���� ���� ����: " + www.downloadHandler.text);
        }
        else
        {
            Debug.LogError("���� ���� ����: " + www.error);
        }
    }
}
