using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    private float attackDelay = 1.5f;
    private float lastAttackTime;

    public void EnterState(EnemyCtrl enemy)
    {
        enemy.currState = EnemyState.ATTACK;
        lastAttackTime = Time.time;
        enemy.navMeshAgent.isStopped = true;
    }

    public void UpdateState(EnemyCtrl enemy)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            // 플레이어가 범위 밖이면 다시 추격
            if (Vector3.Distance(enemy.transform.position, player.transform.position) > enemy.attackRange)
            {
                enemy.navMeshAgent.isStopped = false;
                enemy.ChangeState(new EnemyChaseState());
                return;
            }
            else
            {
                // 그렇지 않으면 공격
                if (Time.time - lastAttackTime >= attackDelay)
                {
                    enemy.Attack();// 를 넣으면 될듯?
                    lastAttackTime = Time.time;
                }
            }

        }
    }

    public void FixedUPdateState(EnemyCtrl enemy)
    {

    }

    public void ExitState(EnemyCtrl enemy)
    {
        // 공격 상태 해제 시 필요한 처리
    }
}

