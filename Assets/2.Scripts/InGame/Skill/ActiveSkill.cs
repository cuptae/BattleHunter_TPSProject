using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;
using UnityEngine.UI;

public abstract class ActiveSkill : ISkill
{
    public ActiveData activeData{get; private set;}
    public bool max;
    protected PlayerCtrl player;
    protected System.Action onSkillEnd;
    public bool isOnCooldown{get; private set;}
    protected int chargeCount;
    protected GameObject projectilePrefab;
    protected GameObject EffectPrefab;
    protected Image icon;
    protected string skillDesc;
    public string nextSkillDesc;
    public static List<GizmoDrawRequest> gizmo = new List<GizmoDrawRequest>();
    Collider[] monsterCols;
    public ActiveSkill(ActiveData activeData,GameObject effectVfx,PlayerCtrl player,Image icon)
    {
        this.activeData = activeData;
        this.player = player;
        this.chargeCount = activeData.chargeCount;
        this.icon = icon;
        this.activeData.SetCaster(player);
        this.skillDesc = activeData.skillDesc;
        this.nextSkillDesc = SkillManager.Instance.GetSkillData(activeData.skillId+1).skillDesc;
        if(activeData.skillType == SKILLCONSTANT.SkillType.PROJECTILE)
        {
            SetProjectile(activeData.skillName,activeData.projectileCount+10);
        }
        AssignSkillIcons();
        isOnCooldown = false;

    }
    public abstract IEnumerator Activation();
    public IEnumerator LevelUp()
    {
        // 다음 레벨의 스킬 ID 계산
        int nextSkillId = activeData.skillId + 1;

        // 다음 레벨의 데이터 로드
        ActiveData nextData = SkillManager.Instance.GetSkillData(nextSkillId);
        if(SkillManager.Instance.GetSkillData(nextSkillId+1) != null)
        {
            string nextSkillDesc = SkillManager.Instance.GetSkillData(nextSkillId+1).skillDesc;
            this.nextSkillDesc = nextSkillDesc;
        }

        if (nextData == null)
        {
            Debug.LogWarning($"No data found for skillId: {nextSkillId}. Max level?");
            yield break;
        }

        // 새로운 데이터로 교체
        this.activeData = nextData;
        this.chargeCount = nextData.chargeCount;
        this.activeData.SetCaster(player);

        // Projectile 재설정 (필요할 경우)
        if (nextData.skillType == SKILLCONSTANT.SkillType.PROJECTILE)
        {
            SetProjectile(nextData.skillName, nextData.projectileCount + 10);
        }

        // 아이콘 재할당 (스킬 레벨이 올라가면 아이콘이 바뀔 수도 있으니까)
        AssignSkillIcons();
        Debug.Log($"{activeData.skillName} Leveled up compelete");
    }

    public void SetOnSkillEndCallback(System.Action callback)
    {
        onSkillEnd = () =>
        {
            callback?.Invoke();
            StartCoolDown();
        };
    }

    protected List<IDamageable> ScanEnemyBox(float angleOffset)
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

        //List<EnemyCtrl> enemys = new List<EnemyCtrl>();
        List<IDamageable> enemys = new List<IDamageable>();
        foreach (Collider col in monCols)
        {
            //EnemyCtrl enemy = col.GetComponent<EnemyCtrl>();
            IDamageable enemy = col.GetComponent<IDamageable>();
            if (enemy != null)
                enemys.Add(enemy);
        }

        return enemys;
    }

    protected List<IDamageable> ScanEnemySphere()
    {
        Vector3 diameter = Vector3.one * activeData.attackRange * 2f; // (x, y, z 모두 지름)
        gizmo.Add(new GizmoDrawRequest(player.transform.position,diameter,Quaternion.identity,4f,GizmoDrawRequest.DrawType.Sphere));

        Collider[] monCols = Physics.OverlapSphere(player.transform.position,activeData.attackRange);
        //List<EnemyCtrl> enemys = new List<EnemyCtrl>();
        List<IDamageable> enemys = new List<IDamageable>();
        foreach (Collider col in monCols)
        {
            //EnemyCtrl enemy = col.GetComponent<EnemyCtrl>();
            IDamageable enemy = col.GetComponent<IDamageable>();
            if (enemy != null)
                enemys.Add(enemy);
        }
        return enemys;
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

    protected void AssignSkillIcons()
    {
        if (icon == null)
        {
            Debug.LogError("Icon is not assigned!");
            return;
        }

        // 아이콘 스프라이트 로드
        string iconPath = activeData.skillIcon;
        Sprite loadedSprite = Resources.Load<Sprite>(iconPath);

        if (loadedSprite == null)
        {
            Debug.Log($"Failed to load icon sprite at path: {iconPath}");
            return;
        }

        // 자식 아이콘에 스프라이트 설정
        icon.sprite = loadedSprite;

        // 부모 아이콘에 동일한 스프라이트 설정
        Image iconBack = icon.transform.parent.GetComponent<Image>();
        Debug.Log(iconBack.gameObject.name);
        if (iconBack != null)
        {
            iconBack.sprite = loadedSprite;
            Debug.Log($"Icon Sprite assigned successfully: {icon.sprite.name}");
        }
        else
        {
            Debug.LogError("Parent icon not found!");
        }

    }

    protected void StartCoolDown()
    {
        isOnCooldown = true;
        icon.fillAmount = 0f;
        player.StartCoroutine(CoolDown(activeData.cooltime));
    }

    IEnumerator CoolDown(float time)
    {
        float elapsedTime = 0f;
        Debug.Log($"Cooldown for {time} seconds.");
        while(elapsedTime < time)
        {
            icon.fillAmount = elapsedTime / time;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        icon.fillAmount = 1f;
        isOnCooldown = false;
        yield return null;
    }
}


