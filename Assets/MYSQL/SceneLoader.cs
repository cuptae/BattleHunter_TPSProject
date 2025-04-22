using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public JobManager jobManager;
    public InputField playerInputField;

    public void OnStartButtonPressed()
    {
        string playerName = playerInputField.text;
        int characterId = jobManager.GetSelectedCharacterId();

        GameStartData.selectedCharacterId = characterId;
        GameStartData.skillIdQ = -1;
        GameStartData.skillIdE = -1;

        StartCoroutine(LoadSkillSlot(playerName, characterId));
    }

    IEnumerator LoadSkillSlot(string player, int characterId)
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
                GameStartData.skillIdQ = wrapper.data.q.id;
                GameStartData.skillIdE = wrapper.data.e.id;
                Debug.Log($"[SceneLoader] Skill Data Loaded: Q = {GameStartData.skillIdQ}, E = {GameStartData.skillIdE}");
            }
            else
            {
                Debug.Log("[SceneLoader] 스킬 슬롯 데이터 없음 → 기본값 사용");
            }
        }
        else
        {
            Debug.LogWarning("[SceneLoader] 스킬 슬롯 불러오기 실패: " + www.error);
        }

        // 씬 이동 없음. 외부에서 LoadScene 따로 실행하세요.
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
