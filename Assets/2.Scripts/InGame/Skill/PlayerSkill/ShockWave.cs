using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : ActiveSkill
{
    public ShockWave(ActiveData activeData,GameObject effectVfx,PlayerCtrl player):base(activeData,effectVfx,player){}
    public override IEnumerator Activation()
    {

        Debug.Log($"skillId: {activeData.skillId},skillName: {activeData.skillName},damage: {activeData.damage},attackDistance: {activeData.attackDistance},attackRange: {activeData.attackRange}");
        player.animator.SetTrigger("QSkill");
        yield return new WaitForSeconds(0.63333f);
        if(ScanEnemyBox() != null)
        {
            foreach(EnemyCtrl enemy in ScanEnemyBox())
            {
                Debug.Log(enemy.transform.name);
                enemy.GetDamage(activeData.damage);
            }
        }

        // 애니메이션 길이만큼 대기
        yield return new WaitForSeconds(1.8f-0.63333f);

        Debug.Log("ShockWave Finished");
        onSkillEnd?.Invoke();
    }

}
