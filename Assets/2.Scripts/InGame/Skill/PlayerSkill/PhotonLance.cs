using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLance : ActiveSkill
{
    public PhotonLance(ActiveData activeData,GameObject effectVfx,GameObject projectile,PlayerCtrl player):base(activeData,effectVfx,projectile,player){}
    public override IEnumerator Activation()
    {
        Debug.Log("포톤랜스 발동");
        player.animator.SetTrigger("RSkill");
        yield return new WaitForSeconds(4.15f);
        Debug.Log("PhotonLance Finished");
        onSkillEnd?.Invoke();
    }
}