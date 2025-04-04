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
    protected GameObject projectilePrefab;
    Collider[] monsterCols;
    public ActiveSkill(ActiveData activeData,GameObject effectVfx,PlayerCtrl player)
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
        Vector3 boxRange = new Vector3(activeData.attackRange,3,activeData.attackDistance);
        Vector3 boxCenter = new Vector3(0, -1, -1 + boxRange.z / 2);
        Vector3 attackPos = player.transform.position + player.transform.forward * 2f + player.transform.up * 2f;
        Quaternion attackRot = player.transform.rotation;

        Vector3 center = attackPos + attackRot * boxCenter;

        Collider[] monCols = Physics.OverlapBox(center, boxRange * 0.5f, attackRot, GameManager.Instance.enemyLayerMask);

        List<EnemyCtrl> enemys = new List<EnemyCtrl>();
        foreach (Collider col in monCols)
        {
            EnemyCtrl enemy = col.GetComponent<EnemyCtrl>();
            if (enemy != null)
                enemys.Add(enemy);
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
        projectilePrefab = Resources.Load<GameObject>(name);

        if (projectilePrefab == null)
        {
            Debug.LogError($"Failed to load prefab at path: {name}");
            return;
        }
        PoolManager.Instance.CreatePhotonPool(name, projectilePrefab, size);
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


