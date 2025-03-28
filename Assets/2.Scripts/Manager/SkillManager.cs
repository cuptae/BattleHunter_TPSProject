using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SkillManager : MonoSingleton<SkillManager>
{

    const string tableUrl = "https://docs.google.com/spreadsheets/d/{0}/export?format=csv&gid={1}";
    public List<Dictionary<string,object>> skillTable;
    public PlayerCtrl player;

    public int skillId;
    protected override void Awake()
    {
        base.Awake();
    }

    IEnumerator Start()
    {
        yield return StartCoroutine(TableDownload());
        // skillId가 10101인 스킬 데이터 찾기

        
        // if (skillData != null)
        // {
        //     Debug.Log(skillData["skillId"]);
        // }

    }

    public List<ActiveSkill> SkillAdd()
    {
        List<ActiveSkill> activeSkills = new List<ActiveSkill>();
        switch(GameManager.Instance.curCharacter)
        {
            case Character.GUNNER:
            activeSkills.Add(new ShockWave(SetSkillData(20101),player));
            activeSkills.Add(new GrenadeLauncher(SetSkillData(20201),player));
            activeSkills.Add(new PhotonLance(SetSkillData(20301),player));
            break;
            case Character.HACKER:
            break;
            case Character.WARRIOR:
            break;
        }
        return activeSkills;
    }
    public ActiveData SetSkillData(int skillId)
    {
        Dictionary<string, object> skillData = GetSkillData(skillId);
        ActiveData  activeData = new ActiveData();
        activeData.SetSkillId((int)skillData["skillId"]);

        return activeData;
    }

    IEnumerator TableDownload()
    {
        string url = string.Format(tableUrl, "1RqpyepNlZmXxQXTSdYI3SnyvlICCJdoExM3akJNIGzc", 0);
        UnityWebRequest req = UnityWebRequest.Get(url);
        yield return req.SendWebRequest(); // 요청이 완료될 때까지 대기

        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Failed to download skill table: {req.error}");
            yield break; // 에러가 있으면 종료
        }

        string text = req.downloadHandler.text;
        skillTable = CSVReader.Read(text);
        Debug.Log("Skill table loaded successfully!");
    }

    public Dictionary<string, object> GetSkillData(int skillId)
    {
        return skillTable.Find(skill =>
        {
            if (skill["skillId"] is int id)
            {
                return id == skillId;
            }
            else if (skill["skillId"] is string idStr && int.TryParse(idStr, out int parsedId))
            {
                return parsedId == skillId;
            }
            return false;
        });
    }

}
