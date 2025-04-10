using System;
using System.Collections;
using System.Collections.Generic;
using SKILLCONSTANT;
using UnityEngine;
using UnityEngine.Networking;

public class SkillManager : MonoSingleton<SkillManager>
{

    const string tableUrl = "https://docs.google.com/spreadsheets/d/{0}/export?format=csv&gid={1}";
    private List<Dictionary<string,object>> skillTable;
    [HideInInspector]
    public PlayerCtrl player;
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
            activeSkills.Add(new ShockWave(GetSkillData(20101),null,player));
            activeSkills.Add(new GrenadeLauncher(GetSkillData(20201),null,player));
            activeSkills.Add(new PhotonLance(GetSkillData(20301),null,player));
            break;
            case Character.HACKER:
            break;
            case Character.WARRIOR:
            activeSkills.Add(new FocusField(GetSkillData(10101),null,player));
            activeSkills.Add(new EnergyBurst(GetSkillData(10201),null,player));
            activeSkills.Add(new EnduranceMode(GetSkillData(10301),null,player));
            break;
        }
        return activeSkills;
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

    public ActiveData GetSkillData(int skillId)
    {
        var skillDict = skillTable.Find(skill =>
        {
            if (skill.TryGetValue("skillId", out object value))
            {
                if (value is int id)
                {
                    return id == skillId;
                }
                if (value is string idStr && int.TryParse(idStr, out int parsedId))
                {
                    return parsedId == skillId;
                }
            }
            return false;
        });

        if (skillDict == null)
        {
            Debug.LogWarning($"Skill ID {skillId} not found.");
            return null;
        }

        ActiveData data = new ActiveData();
        data.SetSkillId(skillId);
        data.SetSkillName(skillDict["name"] as string);
        data.SetSkillDesc(skillDict["desc"] as string);
        data.SetSkillIcon(skillDict["icon"] as string);
        data.SetSkillDamage(Convert.ToInt32(skillDict["damage"]));
        data.SetCooltime(Convert.ToSingle(skillDict["coolTime"]));
        data.SetAttackRange(Convert.ToSingle(skillDict["attackRange"]));
        data.SetAttackDistance(Convert.ToSingle(skillDict["attackDistance"]));
        data.SetIsCharge(Convert.ToBoolean(skillDict["isCharge"]));
        data.SetChargeCount(Convert.ToInt32(skillDict["chargeCount"]));
        data.SetProjectileCount(Convert.ToInt32(skillDict["projectileCount"]));
        data.SetDuration(Convert.ToSingle(skillDict["duration"]));
        data.SetSkillEffectParam(Enum.TryParse(skillDict["skillEffectParam"].ToString(), out SkillEffect effect) ? effect : SkillEffect.NONE);
        data.SetSkillType(Enum.TryParse(skillDict["skillType"].ToString(), out SkillType type) ? type : SkillType.NONE);

        return data;
    }




}
