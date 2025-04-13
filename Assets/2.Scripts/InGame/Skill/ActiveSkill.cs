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
    protected GameObject EffectPrefab;

    public static List<GizmoDrawRequest> gizmo = new List<GizmoDrawRequest>();
    Collider[] monsterCols;
    public ActiveSkill(ActiveData activeData,GameObject effectVfx,PlayerCtrl player)
    {
        this.activeData = activeData;
        this.player = player;
        this.chargeCount = activeData.chargeCount;
        if(activeData.skillType == SKILLCONSTANT.SkillType.PROJECTILE)
        {
            SetProjectile(activeData.skillName,activeData.projectileCount+5);
        }

    }
    public abstract IEnumerator Activation();

    public void SetOnSkillEndCallback(System.Action callback)
    {
        onSkillEnd = callback;
    }

    protected List<EnemyCtrl> ScanEnemyBox(float angleOffset)
    {
        Vector3 boxRange = new Vector3(activeData.attackRange, 3, activeData.attackDistance);
        Vector3 boxCenter = new Vector3(0, 0, -1 + boxRange.z / 2);

        // 원하는 방향으로 회전
        Quaternion attackRot = Quaternion.Euler(0, angleOffset, 0) * player.transform.rotation;

        // 그 방향 기준으로 중심 위치 계산
        Vector3 center = player.transform.position + attackRot * Vector3.forward * 2f + attackRot * boxCenter;
        gizmo.Add(new GizmoDrawRequest(center, boxRange, attackRot, 4f,GizmoDrawRequest.DrawType.Box));

        // 박스 범위 감지
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

    protected List<EnemyCtrl> ScanEnemySphere()
    {
        Vector3 diameter = Vector3.one * activeData.attackRange * 2f; // (x, y, z 모두 지름)
        gizmo.Add(new GizmoDrawRequest(player.transform.position,diameter,Quaternion.identity,4f,GizmoDrawRequest.DrawType.Sphere));

        Collider[] monCols = Physics.OverlapSphere(player.transform.position,activeData.attackRange);
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


