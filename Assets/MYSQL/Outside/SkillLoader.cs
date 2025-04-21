using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Skill
{
    public int id;                // ← 실제 스킬 ID
    public string skill_name;
    public string image_name;
}

[System.Serializable]
public class SkillListWrapper
{
    public Skill[] Items;
}

public class SkillLoader : MonoBehaviour
{
    private int characterId = -1;

    [Header("스킬 아이콘이 들어갈 이미지 슬롯들")]
    public Image[] skillImageSlots;

    [Header("인스펙터 확인용 저장 배열")]
    public int[] loadedSkillIds;
    public Sprite[] loadedSkillSprites;

    public void SetCharacterId(int id)
    {
        characterId = id;
        Debug.Log("[SkillLoader] 선택된 캐릭터 ID 저장됨: " + characterId);
    }

    public void OnClick_LoadSkills()
    {
        if (characterId > 0)
        {
            Debug.Log("[SkillLoader] 스킬 로딩 시도 (ID: " + characterId + ")");
            StartCoroutine(LoadSkillsFromDB());
        }
        else
        {
            Debug.LogWarning("캐릭터 ID가 설정되지 않았습니다.");
        }
    }

    IEnumerator LoadSkillsFromDB()
    {
        string url = "http://192.168.0.24:8080/get_skills.php?character_id=" + characterId;
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string json = "{\"Items\":" + www.downloadHandler.text + "}";
            SkillListWrapper skillData = JsonUtility.FromJson<SkillListWrapper>(json);

            loadedSkillIds = new int[skillImageSlots.Length];
            loadedSkillSprites = new Sprite[skillImageSlots.Length];

            for (int i = 0; i < skillImageSlots.Length; i++)
            {
                if (i < skillData.Items.Length)
                {
                    Skill skill = skillData.Items[i];
                    Sprite sprite = LoadSpriteFromSheet("skills", skill.image_name);

                    skillImageSlots[i].sprite = sprite;

                    // 실제 스킬 ID 저장!
                    loadedSkillIds[i] = skill.id;
                    loadedSkillSprites[i] = sprite;

                    Debug.Log($"[SkillLoader] 슬롯 {i}에 스킬 ID {skill.id} - {skill.skill_name} 로드됨");
                }
                else
                {
                    skillImageSlots[i].sprite = null;
                    loadedSkillIds[i] = -1;
                    loadedSkillSprites[i] = null;
                }
            }
        }
        else
        {
            Debug.LogError("스킬 불러오기 실패: " + www.error);
        }
    }

    public Sprite LoadSpriteFromSheet(string sheetName, string spriteName)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>(sheetName);
        foreach (Sprite s in sprites)
        {
            if (s.name == spriteName)
                return s;
        }
        Debug.LogWarning("스프라이트 없음: " + spriteName);
        return null;
    }

}
