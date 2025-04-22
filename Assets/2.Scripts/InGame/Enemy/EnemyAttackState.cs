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
        if(enemy.pv.isMine)
            enemy.navMeshAgent.isStopped = true;
    }

    public void UpdateState(EnemyCtrl enemy)
    {

            Vector3 direction = (enemy.targetPlayer.position - enemy.transform.position).normalized;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lookRotation, enemy.rotationSpeed * Time.deltaTime);
        if (enemy.targetPlayer == null)
            return;

        if (enemy is Dragoon dragoon)
        {
            float distance = Vector3.Distance(dragoon.transform.position, dragoon.targetPlayer.position);

            // 가까이 붙으면 후퇴 위해 Chase 상태로 다시 전환
            if (distance < dragoon.stopDistance - dragoon.bufferDistance)
            {
                enemy.ChangeState(new EnemyChaseState());
                return;
            }
            else if (dragoon.pv.isMine)
            {
                // 공격 거리 유지하면 멈춤
                dragoon.navMeshAgent.ResetPath();
            }
        }

        // 공격 사거리 벗어났으면 다시 Chase
        if (Vector3.Distance(enemy.transform.position, enemy.targetPlayer.position) > enemy.attackRange + 1)
        {
            if (enemy.pv.isMine)
                enemy.navMeshAgent.isStopped = false;

            enemy.ChangeState(new EnemyChaseState());
            return;
        }

        // 공격 가능하면 공격
        if (Time.time - lastAttackTime >= attackDelay)
        {
            Debug.Log("Dragoon Attack");
            enemy.Attack();
            lastAttackTime = Time.time;
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

