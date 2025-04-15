using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockBackState : IEnemyState
{
    Transform caster;
    Vector3 dir;
    Vector3 startPos;
    Vector3 endPos;
    float distance = 5.0f;

    float time = 0f;
    float duration = 2.0f;
    #pragma warning restore format
    public EnemyKnockBackState(Transform caster)
    {
        this.caster = caster;
    }
    public void EnterState(EnemyCtrl enemy)
    { 
        enemy.navMeshAgent.enabled = false;
        dir = (enemy.transform.position - caster.position).normalized;
        startPos = enemy.transform.position;
        endPos = startPos + dir * distance;
    }

    public void UpdateState(EnemyCtrl enemy)
    {
        if(time<duration)
        {
            enemy.transform.position = Vector3.Lerp(startPos, endPos, time / duration);
            time += Time.deltaTime;
        }

        if(Vector3.Distance(enemy.transform.position,endPos)<0.1f)
        {
            enemy.ChangeState(new EnemyChaseState());
        }
    }

    public void FixedUPdateState(EnemyCtrl enemy)
    {

    }

    public void ExitState(EnemyCtrl enemy)
    {

    }


}
