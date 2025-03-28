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
