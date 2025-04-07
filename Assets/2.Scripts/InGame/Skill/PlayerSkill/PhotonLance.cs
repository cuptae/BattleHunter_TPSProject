using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLance : ActiveSkill
{
    Transform firePos;

    public PhotonLance(ActiveData activeData,GameObject effectVfx,PlayerCtrl player):base(activeData,effectVfx,player)
    {
        firePos = player.transform.Find("Sci_Fi_Character_08_03/root/pelvis/spine_01/spine_02/spine_03/clavicle_r/upperarm_r/lowerarm_r/hand_r/Riple/SciFiGunLightWhite/GrenadeFirePos");
        effectVfx = Resources.Load<GameObject>(activeData.skillName+"Vfx");
        PoolManager.Instance.CreatePool(activeData.skillName+"Vfx",effectVfx,3);
    }
    public override IEnumerator Activation()
    {
        Debug.Log("포톤랜스 발동");
        player.animator.SetTrigger("RSkill");
        yield return new WaitForSeconds(1.15f);
        float duration = 117f / 60f - 69f / 60f; // = 0.8초
        float timer = 0f;
        float interval = 0.1f; // 스캔 주기
        GameObject effect = PoolManager.Instance.GetObject(activeData.skillName+"Vfx",firePos.position,Quaternion.LookRotation(firePos.forward));
        if(activeData.projectileCount == 2)
        {
            while (timer < duration)
            {
                float t = timer / duration; // 0~1로 정규화
                float leftAngle = Mathf.Lerp(-40f, 0f, t);
                float rightAngle = Mathf.Lerp(40f, 0f, t);

                List<EnemyCtrl> leftEnemies = ScanEnemyBox(leftAngle);
                List<EnemyCtrl> rightEnemies = ScanEnemyBox(rightAngle);

                foreach (EnemyCtrl enemy in leftEnemies)
                {
                    enemy.GetDamage(activeData.damage);
                }

                foreach (EnemyCtrl enemy in rightEnemies)
                {
                    enemy.GetDamage(activeData.damage);
                }

                timer += interval;
                yield return new WaitForSeconds(interval);
            }
        }
        else
        { 
            while (timer < duration)
            {
                float t = timer / duration; // 0~1로 정규화

                List<EnemyCtrl> enemies = ScanEnemyBox(0);

                foreach (EnemyCtrl enemy in enemies)
                {
                    enemy.GetDamage(activeData.damage);
                }

                timer += interval;
                yield return new WaitForSeconds(interval);
            }
        }
        yield return new WaitForSeconds(2.3f);
        Debug.Log("PhotonLance Finished");
        //effect.GetComponent<>
        onSkillEnd?.Invoke();
    }
}