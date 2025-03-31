using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

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

    protected List<EnemyCtrl> ScanEnemyBox()
    {
        Vector3 boxRange = new Vector3(activeData.attackRange,2f,activeData.attackDistance);
        Vector3 attackPos = player.transform.position+player.transform.forward*1f+player.transform.up*2f;
        Quaternion attackRot = player.transform.rotation;

        Collider [] monCols = Physics.OverlapBox(attackPos,boxRange,attackRot,GameManager.Instance.enemyLayerMask);

        List<EnemyCtrl> enemys = new List<EnemyCtrl>();

        foreach(Collider col in monCols)
        {
            enemys.Add(col.GetComponent<EnemyCtrl>());
        }
        return enemys;
    }

    protected void Effect()
    {
        switch(activeData.skillEffectParam)
        {
            case SKILLCONSTANT.SkillEffect.KNUCKBACK:
                
            break;
        }
    }
    


}


