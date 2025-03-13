using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : IEnemyState
{
    private float attackCooldown = 1.5f;
    private float lastAttackTime;

    public void EnterState(EnemyCtrl enemy)
    {
        lastAttackTime = Time.time;
        enemy.navMeshAgent.isStopped = true;
    }

    public void UpdateState(EnemyCtrl enemy)
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            // 플레이어가 범위 밖이면 다시 추격
            if (Vector3.Distance(enemy.transform.position, player.transform.position) > 3.0f)
            {
                enemy.navMeshAgent.isStopped = false;
                enemy.ChangeState(new ChaseState());
                return;
            }
            else
            {
                // 공격
                if (Time.time - lastAttackTime >= attackCooldown)
                {
                    Debug.Log("Enemy attacks!");
                    lastAttackTime = Time.time;
                }
            }

        }
    }

    public void ExitState(EnemyCtrl enemy)
    {
        // 공격 상태 해제 시 필요한 처리
    }
}

