using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Animations.Rigging;

public class EnemyKnockBackState : IEnemyState
{
    Transform caster;
    Vector3 dir;
    Vector3 startPos;
    Vector3 endPos;
    float distance = 5.0f;

    float time = 0f;
    float duration = 0.5f;
    


    public EnemyKnockBackState(Transform caster)
    {
        this.caster = caster;
    }
    public void EnterState(EnemyCtrl enemy)
    { 
        enemy.currState = EnemyState.KNOCKBACK;
        enemy.navMeshAgent.enabled = false;
        enemy.GetComponentInChildren<RigBuilder>().enabled = false;
        dir = (enemy.transform.position - caster.position).normalized;
        startPos = enemy.transform.position;
        endPos = startPos + dir * distance;
    }

    public void UpdateState(EnemyCtrl enemy)
    {
        time += Time.deltaTime;
        float t = Mathf.Clamp01(time / duration);

        bool isBlocked = Physics.Raycast(enemy.transform.position, dir, out RaycastHit hitInfo, 1.0f, GameManager.Instance.groundLayer);

        if (!isBlocked)
        {
            enemy.transform.position = Vector3.Lerp(startPos, endPos, t);
        }

        if (t >= 1f)
        {
            enemy.navMeshAgent.enabled = true;
            enemy.pv.RPC("RPC_EnableRigBuilder", PhotonTargets.All);
            //enemy.GetComponentInChildren<RigBuilder>().enabled = true;
            enemy.ChangeState(new EnemyChaseState());
        }
    }

    public void FixedUPdateState(EnemyCtrl enemy)
    {

    }

    public void ExitState(EnemyCtrl enemy)
    {
        enemy.pv.RPC("RPC_EnableRigBuilder", PhotonTargets.All);
        enemy.navMeshAgent.enabled =true;
    }


}
