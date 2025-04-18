using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillState : PlayerState
{
    ActiveSkill curSkill;
    bool isDone;
    public SkillState(PlayerCtrl player,ActiveSkill skill) : base(player){ curSkill = skill;}

    public override void EnterState()
    {
        isDone = false;
        int hundredsPlace = (curSkill.activeData.skillId / 100) % 10;
        // switch (hundredsPlace)
        // {
        //     case 1:
        //         player.qSkillTrigger = true;
        //         player.animator.SetTrigger("QSkill");
        //         break;
        //     case 2:
        //         player.eSkillTrigger = true;
        //         player.animator.SetTrigger("ESkill");
        //         break;
        //     case 3:
        //         player.rSkillTrigger = true;
        //         player.animator.SetTrigger("RSkill");
        //         break;
        //     default:
        //         Debug.LogWarning($"Unhandled skillId: {curSkill.activeData.skillId}");
        //         break;
        // }
        player.pv.RPC("RPC_PlaySkillAnim", PhotonTargets.AllBuffered, hundredsPlace);
        
        curSkill.SetOnSkillEndCallback(() => isDone = true);
        player.StartCoroutine(curSkill.Activation());
    }
    public override void UpdateState()
    {
        if (isDone) 
        {
            player.ChangeState(new IdleState(player));
        }
    }
    public override void FixedUpdateState()
    {

    }
    public override void ExitState()
    {

    }
    
}
