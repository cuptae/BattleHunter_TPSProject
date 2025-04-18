using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ShockWave : ActiveSkill
{
    Transform firePos;
    public ShockWave(ActiveData activeData,GameObject effectVfx,PlayerCtrl player,Image icon):base(activeData,effectVfx,player,icon)
    {
        firePos = player.transform.Find("Sci_Fi_Character_08_03/root/pelvis/spine_01/spine_02/ShotHolster/ShotGun/FirePos");
        if (activeData.projectileCount == 1)
        effectVfx = Resources.Load<GameObject>(activeData.skillName+"Vfx");
        PoolManager.Instance.CreatePool(activeData.skillName+"Vfx",effectVfx,3);
    }
    public override IEnumerator Activation()
    {
        if (isOnCooldown) yield break;

        Debug.Log($"skillId: {activeData.skillId},skillName: {activeData.skillName},damage: {activeData.damage},attackDistance: {activeData.attackDistance},attackRange: {activeData.attackRange}");
        yield return new WaitForSeconds(0.4222f);
        PoolManager.Instance.GetObject(activeData.skillName+"Vfx",firePos.position,Quaternion.LookRotation(firePos.forward));
        if(ScanEnemyBox(0) != null)
        {
            foreach(IDamageable enemy in ScanEnemyBox(0))
            {
                enemy.GetDamage(activeData.damage,activeData);
            }
        }

        // 애니메이션 길이만큼 대기
        yield return new WaitForSeconds(0.7777f); // 나머지 애니메이션 시간

        Debug.Log("ShockWave Finished");
        onSkillEnd?.Invoke();
    }

}
