using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnduranceMode : ActiveSkill
{
    public EnduranceMode(ActiveData activeData,GameObject effectVfx,PlayerCtrl player,Image icon):base(activeData,effectVfx,player,icon)
    {
        effectVfx = Resources.Load<GameObject>(activeData.skillName+"Vfx");
        PoolManager.Instance.CreatePool(activeData.skillName+"Vfx",effectVfx,3);
    }

    public override IEnumerator Activation()
    {
        if (isOnCooldown) yield break;
        GameObject effectObj = PoolManager.Instance.GetObject(activeData.skillName+"Vfx",player.transform.position,Quaternion.identity);
        Transform temp = effectObj.transform.parent;
        effectObj.transform.SetParent(player.transform);
        effectObj.transform.localPosition = new Vector3(0, 1f, 0);
        effectObj.transform.localRotation = Quaternion.identity;
        player.SetInvincible(true);
        onSkillEnd?.Invoke();
        yield return new WaitForSeconds(activeData.duration);
        PoolManager.Instance.ReturnObject(activeData.skillName+"Vfx", effectObj);
        effectObj.transform.SetParent(temp.transform);
        player.SetInvincible(false);
        Debug.Log("EnduranceMode Finished");
    }
}
