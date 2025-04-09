using System.Collections;
using UnityEngine;

public class GrenadeLauncher : ActiveSkill
{
    float totalAnim = 1.18f;
    float attackTiming = 0.85f;
    Transform firePos;
    float[] angles;
    public GrenadeLauncher(ActiveData activeData,GameObject effectVfx,PlayerCtrl player):base(activeData,effectVfx,player)
    {
        firePos = player.transform.Find("Sci_Fi_Character_08_03/root/pelvis/spine_01/spine_02/spine_03/clavicle_r/upperarm_r/lowerarm_r/hand_r/Riple/SciFiGunLightWhite/GrenadeFirePos");
        effectVfx = Resources.Load<GameObject>(activeData.skillName+"Vfx");
        PoolManager.Instance.CreatePhotonPool(activeData.skillName+"Vfx"+GameManager.Instance.userId,effectVfx,3);
        if (activeData.projectileCount == 1)
            angles = new float[] { 0f };
        else if (activeData.projectileCount == 3)
            angles = new float[] { -5f, 0f, 5f };
    }
    public override IEnumerator Activation()
    {
        player.animator.SetTrigger("ESkill");
        yield return new WaitForSeconds(attackTiming);
        foreach (float angle in angles)
        {
            // Y축 기준으로 회전 적용
            Quaternion rotation = Quaternion.Euler(0, angle, 0) * player.transform.rotation;
            Vector3 direction = rotation * Vector3.forward;
            GameObject projectile = SpawnProjectile(firePos.position, Quaternion.identity);
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



