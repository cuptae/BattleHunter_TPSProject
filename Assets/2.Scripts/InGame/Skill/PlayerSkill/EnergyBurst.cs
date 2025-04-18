using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyBurst : ActiveSkill
{
    public EnergyBurst(ActiveData activeData,GameObject effectVfx,PlayerCtrl player,Image icon):base(activeData,effectVfx,player,icon)
    {
        effectVfx = Resources.Load<GameObject>(activeData.skillName+"Vfx");
        PoolManager.Instance.CreatePool(activeData.skillName+"Vfx",effectVfx,3);
    }

    public override IEnumerator Activation()
    {
        if (isOnCooldown) yield break;
        PoolManager.Instance.GetObject(activeData.skillName+"Vfx",player.transform.position,Quaternion.identity);
        if(ScanEnemySphere() != null)
        {
            foreach(EnemyCtrl enemy in ScanEnemySphere())
            {
                enemy.GetDamage(activeData.damage,activeData);
            }
        }
        yield return new WaitForSeconds(0.1f);
        Debug.Log("EnergeBurst Finished");
        onSkillEnd?.Invoke();
    }
}
