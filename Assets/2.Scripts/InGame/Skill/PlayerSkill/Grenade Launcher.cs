using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeLauncher : ActiveSkill
{
    float totalAnim = 1.18f;
    float attackTiming = 0.85f;
    Transform firePos;
    float[] angles;
    public GrenadeLauncher(ActiveData activeData,GameObject effectVfx,PlayerCtrl player,Image icon):base(activeData,effectVfx,player,icon)
    {
        firePos = player.transform.Find("Sci_Fi_Character_08_03/root/pelvis/spine_01/spine_02/spine_03/clavicle_r/upperarm_r/lowerarm_r/hand_r/Riple/SciFiGunLightWhite/GrenadeFirePos");
        effectVfx = Resources.Load<GameObject>(activeData.skillName+"Vfx");
        if(player.pv.isMine)
        {
            PoolManager.Instance.CreatePhotonPool(activeData.skillName+"Vfx",effectVfx,3);
        }
    }
    public override IEnumerator Activation()
    {
        if (isOnCooldown) yield break;
        if (activeData.projectileCount == 1)
            angles = new float[] { 0f };
        else if (activeData.projectileCount == 3)
            angles = new float[] { -5f, 0f, 5f };
        yield return new WaitForSeconds(attackTiming);
        foreach (float angle in angles)
        {
            Debug.Log(activeData.projectileCount);
            // Y축 기준으로 회전 적용
            Quaternion rotation = Quaternion.Euler(0, angle, 0) * player.transform.rotation;
            Vector3 direction = rotation * Vector3.forward;
            GameObject projectile = SpawnProjectile(firePos.position, Quaternion.identity);
            if (projectile == null)
            {
                Debug.LogError("Projectile이 null입니다! SpawnProjectile() 확인 요망");
                yield break;
            }
            projectile.GetComponent<ExpolsionSkillProjectile>().SetProjectileData(activeData);
            projectilePrefab.transform.position = firePos.position;
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(direction * 25f, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(totalAnim - attackTiming);
        Debug.Log("Grenade end");
        onSkillEnd?.Invoke();
    }
}



