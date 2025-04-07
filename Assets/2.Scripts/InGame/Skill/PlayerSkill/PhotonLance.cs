using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLance : ActiveSkill
{
    Transform firePos;
    float[] angles;

    public PhotonLance(ActiveData activeData,GameObject effectVfx,PlayerCtrl player):base(activeData,effectVfx,player)
    {
        firePos = player.transform.Find("Sci_Fi_Character_08_03/root/pelvis/spine_01/spine_02/spine_03/clavicle_r/upperarm_r/lowerarm_r/hand_r/Riple/SciFiGunLightWhite/GrenadeFirePos");
        if (activeData.projectileCount == 1)
            angles = new float[] { 0f };
        else if (activeData.projectileCount == 2)
            angles = new float[] { -10f, 10f };
    }
    public override IEnumerator Activation()
    {
        Debug.Log("포톤랜스 발동");
        player.animator.SetTrigger("RSkill");
        foreach (float angle in angles)
        {
            // Y축 기준으로 회전 적용
            Quaternion rotation = Quaternion.Euler(0, angle, 0) * player.transform.rotation;
            Vector3 direction = rotation * Vector3.forward;

            GameObject projectile = SpawnProjectile(firePos.position, Quaternion.identity);

        }
        yield return new WaitForSeconds(4.15f);
        Debug.Log("PhotonLance Finished");
        onSkillEnd?.Invoke();
    }
}