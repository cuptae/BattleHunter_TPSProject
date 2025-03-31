using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class ActiveSkill : ISkill
{
    protected ActiveData activeData;
    protected PlayerCtrl player;
    protected System.Action onSkillEnd;
    Collider[] monsterCols;
    public ActiveSkill(ActiveData activeData,PlayerCtrl player)
    {
        this.activeData = activeData;
        this.player = player;
    }
    public abstract IEnumerator Activation();

    public void SetOnSkillEndCallback(System.Action callback)
    {
        onSkillEnd = callback;
    }

    protected void SpawnHitBox()
    {
        //monsterCols = Physics.OverlapBox(player.transform,)
    }

    protected void SpawnHitSphere()
    {

    }

    protected void SpawnEffect()
    {
        
    }

}
