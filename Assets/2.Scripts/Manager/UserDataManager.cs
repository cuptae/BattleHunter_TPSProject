using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager Instance;

    [System.Serializable]
    public class InventoryItem
    {
        public int itemId;
        public int quantity;

        public InventoryItem(int id, int qty)
        {
            itemId = id;
            quantity = qty;
        }
    }

    [System.Serializable]
    public class SkillData
    {
        public int skillId;
        public int level;

        public SkillData(int id, int lvl = 1)
        {
            skillId = id;
            level = lvl;
        }
    }

    [System.Serializable]
    public class UserData
    {
        public int level = 1;
        public int gold = 0;
        public List<InventoryItem> inventory = new();
        public List<SkillData> skills = new();
    }

    public UserData userData = new();

    private string filePath => Application.persistentDataPath + "/userData.json";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(userData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("✅ 저장 완료: " + filePath);
    }

    public void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            userData = JsonUtility.FromJson<UserData>(json);
            Debug.Log("✅ 불러오기 완료");
        }
        else
        {
            Debug.Log("⚠ 저장된 데이터 없음. 새로 생성합니다.");
            userData = new UserData();
        }
    }

    // 💡 스킬 강화 예시
    public void UpgradeSkill(int skillId)
    {
        var skill = userData.skills.Find(s => s.skillId == skillId);
        if (skill != null)
        {
            if (skill.level < userData.level)
            {
                skill.level++;
                Debug.Log($"스킬 {skillId} 레벨업! 현재 레벨: {skill.level}");
            }
            else
            {
                Debug.Log("⚠ 유저 레벨보다 높게 스킬을 올릴 수 없습니다.");
            }
        }
        else
        {
            userData.skills.Add(new SkillData(skillId));
            Debug.Log($"새 스킬 {skillId} 추가");
        }

        SaveData();
    }

    // 💡 인벤토리 추가 예시
    public void AddItem(int itemId, int quantity)
    {
        var item = userData.inventory.Find(i => i.itemId == itemId);
        if (item != null)
        {
            item.quantity += quantity;
        }
        else
        {
            userData.inventory.Add(new InventoryItem(itemId, quantity));
        }

        SaveData();
    }
}
