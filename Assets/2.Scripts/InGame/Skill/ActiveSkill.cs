using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveSkill : ISkill
{
    List<Dictionary<string, object>> skillTable =  new List<Dictionary<string,object>>();
    public IEnumerator Activation()
    {
        yield return null;
    }
    public IEnumerator SkillActivation()
    {
        yield return null;
    }
    public void SkillLevelUp()
    {

    }
    public void SetSkillData(int skillId)
    {
        
    }
}
