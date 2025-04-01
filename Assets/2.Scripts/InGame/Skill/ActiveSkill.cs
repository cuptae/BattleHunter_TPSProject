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
    protected bool isActivate;
    protected int chargeCount;
    Collider[] monsterCols;
    public ActiveSkill(ActiveData activeData,GameObject effectVfx,GameObject projectile,PlayerCtrl player)
    {
        this.activeData = activeData;
        this.player = player;
        this.chargeCount = activeData.chargeCount;

        if(activeData.skillType == SKILLCONSTANT.SkillType.PROJECTILE)
        {
            SetProjectile(activeData.skillName,activeData.projectileCount);
        }

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

    protected void Effect(EnemyCtrl enemy)
    {
        switch(activeData.skillEffectParam)
        {
            case SKILLCONSTANT.SkillEffect.KNUCKBACK:
                
            break;

            case SKILLCONSTANT.SkillEffect.STUN:

            break;

            case SKILLCONSTANT.SkillEffect.SLOW:

            break;


        }
    }
    
    public void SetProjectile(string name,int size)
    {
        GameObject prefab = Resources.Load<GameObject>(name);

        if (prefab == null)
        {
            Debug.LogError($"Failed to load prefab at path: {name}");
            return;
        }

        if (PoolManager.Instance == null)
        {
            Debug.LogError("PoolManager Instance is null!");
            return;
        }

        PoolManager.Instance.CreatePhotonPool(name, prefab, size);
    }
    
    protected GameObject SpawnProjectile(Vector3 spawnPos, Quaternion rot)
    {
        string name = activeData.skillName;
        return PoolManager.Instance.GetObject(name,spawnPos,rot);
    }

    protected void CoolDown()
    {
        
    }
    


}


