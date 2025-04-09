using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLance : ActiveSkill
{
    Transform firePos;
    float duration = 117f / 60f - 69f / 60f;

    public PhotonLance(ActiveData activeData,GameObject effectVfx,PlayerCtrl player):base(activeData,effectVfx,player)
    {
        //firePos = player.transform.Find("Sci_Fi_Character_08_03/root/pelvis/spine_01/spine_02/SniperHolster/Sniper_Rifle_03/FirePos");
        firePos = player.transform.Find("Sci_Fi_Character_08_03/PhotonLancePos");
        effectVfx = Resources.Load<GameObject>(activeData.skillName+"Vfx");
        PoolManager.Instance.CreatePhotonPool(activeData.skillName+"Vfx",effectVfx,3);
    }
    public override IEnumerator Activation()
    {
        Debug.Log("포톤랜스 발동");
        player.animator.SetTrigger("RSkill");

        yield return new WaitForSeconds(1.15f);

        float interval = 0.1f;

        if (activeData.projectileCount == 2)
        {
            GameObject rightEffect = PoolManager.Instance.GetObject(activeData.skillName + "Vfx", firePos.position, Quaternion.LookRotation(firePos.forward));
            GameObject leftEffect = PoolManager.Instance.GetObject(activeData.skillName + "Vfx", firePos.position, Quaternion.LookRotation(firePos.forward));
            player.StartCoroutine(DealDamageCoroutine(duration, interval));
            float timer = 0f;
            while (timer < duration)
            {
                float t = timer / duration;
                float leftAngle = Mathf.Lerp(-40f, 0f, t);
                float rightAngle = Mathf.Lerp(40f, 0f, t);

                Vector3 leftDir = Quaternion.AngleAxis(leftAngle, Vector3.up) * player.transform.forward;
                Vector3 rightDir = Quaternion.AngleAxis(rightAngle, Vector3.up) * player.transform.forward;

                leftEffect.transform.rotation = Quaternion.LookRotation(leftDir);
                rightEffect.transform.rotation = Quaternion.LookRotation(rightDir);

                timer += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            GameObject effect = PoolManager.Instance.GetObject(activeData.skillName + "Vfx", firePos.position, Quaternion.LookRotation(firePos.forward));
            float timer = 0f;
            while (timer < duration)
            {
                List<EnemyCtrl> enemies = ScanEnemyBox(0);
                foreach (EnemyCtrl enemy in enemies)
                    enemy.GetDamage(activeData.damage);

                timer += interval;
                yield return new WaitForSeconds(interval);
            }
        }

        yield return new WaitForSeconds(2.25f);
        Debug.Log("PhotonLance Finished");
        onSkillEnd?.Invoke();
    }
        IEnumerator DealDamageCoroutine(float duration, float interval)
        {
            float timer = 0f;
            while (timer < duration)
            {
                float t = timer / duration;
                float leftAngle = Mathf.Lerp(-40f, 0f, t);
                float rightAngle = Mathf.Lerp(40f, 0f, t);

                List<EnemyCtrl> leftEnemies = ScanEnemyBox(leftAngle);
                List<EnemyCtrl> rightEnemies = ScanEnemyBox(rightAngle);

                foreach (EnemyCtrl enemy in leftEnemies)
                    enemy.GetDamage(activeData.damage);

                foreach (EnemyCtrl enemy in rightEnemies)
                    enemy.GetDamage(activeData.damage);

                timer += interval;
                yield return new WaitForSeconds(interval);
            }
        }
    }


