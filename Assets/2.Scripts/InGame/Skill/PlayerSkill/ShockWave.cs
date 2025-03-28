using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : ActiveSkill
{
    public ShockWave(ActiveData activeData,PlayerCtrl player):base(activeData,player){}
    public override IEnumerator Activation()
    {
        Debug.Log("ShockWave Activate");
        player.animator.SetTrigger("QSkill");
        

        // 애니메이션 길이만큼 대기
        yield return new WaitForSeconds(1.8f);

        Debug.Log("ShockWave Finished");
        onSkillEnd?.Invoke();
    }

}
