using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class SkillPanelOpener : MonoBehaviour
{
    public GameObject panel;
    public SkillLoader skillLoader;
    public SkillSlotManager slotManager;
    public JobManager jobManager;
    public InputField playerInputField;

    public void OnClick_OpenSkillPanel()
    {
        panel.SetActive(true);
        skillLoader.OnClick_LoadSkills();

        string playerName = playerInputField.text;
        int characterId = jobManager.GetSelectedCharacterId();

        // 공백 입력 시 기본 이름 지정
        if (string.IsNullOrWhiteSpace(playerName))
        {
            playerName = "default_player";
        }

        if (characterId != -1)
        {
            string encodedPlayer = UnityWebRequest.EscapeURL(playerName);
            StartCoroutine(LoadSavedSkills(encodedPlayer, characterId));
        }
        else
        {
            Debug.LogWarning("캐릭터를 선택해주세요.");
        }
    }

    IEnumerator LoadSavedSkills(string player, int characterId)
    {
        string url = $"http://192.168.0.24:8080/get_skill_slots.php?player={player}&character_id={characterId}";
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string json = www.downloadHandler.text;
            SkillSlotDataWrapper wrapper = JsonUtility.FromJson<SkillSlotDataWrapper>(json);

            if (wrapper != null && wrapper.data != null)
            {
                int qId = wrapper.data.q.id;
                int eId = wrapper.data.e.id;

                slotManager.ResetSlots();
                if (qId > 0)
                    slotManager.SetSkill(qId, skillLoader.LoadSpriteFromSheet("skills", wrapper.data.q.image));
                if (eId > 0)
                    slotManager.SetSkill(eId, skillLoader.LoadSpriteFromSheet("skills", wrapper.data.e.image));

                slotManager.SetInitialSkillSlots(qId, eId);
            }
        }
        else
        {
            Debug.LogError("스킬 슬롯 불러오기 실패: " + www.error);
        }
    }

    [System.Serializable]
    public class SkillSlotDetail
    {
        public int id;
        public string image;
    }

    [System.Serializable]
    public class SkillSlotData
    {
        public SkillSlotDetail q;
        public SkillSlotDetail e;
    }

    [System.Serializable]
    public class SkillSlotDataWrapper
    {
        public SkillSlotData data;
    }
}
