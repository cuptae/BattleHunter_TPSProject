using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerUploader : MonoBehaviour
{
    [Header("UI 연결")]
    public InputField playerInputField;     // 플레이어 이름 입력 필드
    public Button uploadButton;             // 업로드 버튼

    [Header("게임 로직 연결")]
    public JobManager jobManager;           // 캐릭터 ID 받아오기
    public SkillSlotManager slotManager;    // Q, E 슬롯에서 스킬 ID 받아오기

    void Start()
    {
        uploadButton.onClick.AddListener(OnUploadClicked);
    }

    void OnUploadClicked()
    {
        string playerName = playerInputField.text;
        int characterId = jobManager.GetSelectedCharacterId();

        // 공백 또는 null이면 default_player로 설정
        if (string.IsNullOrWhiteSpace(playerName))
        {
            playerName = "default_player";
        }

        if (characterId == -1)
        {
            Debug.LogWarning("캐릭터를 선택해주세요.");
            return;
        }

        List<int> skills = slotManager.GetSelectedSkillIds();

        int skillQ = skills.Count > 0 ? skills[0] : -1;
        int skillE = skills.Count > 1 ? skills[1] : -1;

        Debug.Log($"전송할 데이터 → 이름: {playerName}, 캐릭터 ID: {characterId}, Q: {skillQ}, E: {skillE}");
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
            Debug.Log("서버 전송 성공: " + www.downloadHandler.text);
        }
        else
        {
            Debug.LogError("서버 전송 실패: " + www.error);
        }
    }
}
