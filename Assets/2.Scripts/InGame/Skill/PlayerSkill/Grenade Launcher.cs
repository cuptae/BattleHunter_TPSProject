using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GrenadeLauncher : ActiveSkill
{
    float totalAnim = 1.18f;
    float attackTiming = 0.83f;
    Transform firePos;
    public GrenadeLauncher(ActiveData activeData,GameObject effectVfx,GameObject projectile,PlayerCtrl player):base(activeData,effectVfx,projectile,player){}
    public override IEnumerator Activation()
    {
        firePos = player.transform.Find("Sci_Fi_Character_08_03/root/pelvis/spine_01/spine_02/spine_03/clavicle_r/upperarm_r/lowerarm_r/hand_r/Riple/SciFiGunLightWhite/GrenadeFirePos");

        Debug.Log("그레네이드 런처 Activate");
        player.animator.SetTrigger("ESkill");
        yield return new WaitForSeconds(attackTiming);
        Debug.Log("수류탄 발사");
        GameObject projectile = SpawnProjectile(firePos.position,Quaternion.identity);
        projectile.GetComponent<Rigidbody>().AddForce(player.transform.forward*50f,ForceMode.Impulse);
        yield return new WaitForSeconds(totalAnim-attackTiming);
        Debug.Log("Grenade end");
        PoolManager.Instance.ReturnObject(activeData.skillName,projectile);
        projectile.transform.position = firePos.transform.position;
        projectile.GetComponent<Rigidbody>().velocity = Vector3.zero;
        projectile.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        onSkillEnd?.Invoke();
   
        
    }
}