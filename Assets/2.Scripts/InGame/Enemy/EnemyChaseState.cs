using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyChaseState : IEnemyState
{
    public void EnterState(EnemyCtrl enemy)
    {
        enemy.currState = EnemyState.CHASE;
        if(enemy.pv.isMine)
            enemy.navMeshAgent.isStopped = false;
        
    }

    public void UpdateState(EnemyCtrl enemy)
    {
    if (enemy.targetPlayer == null)
        return;

    if (enemy.pv.isMine)
    {
            Vector3 direction = (enemy.targetPlayer.position - enemy.transform.position).normalized;
            direction.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lookRotation, enemy.rotationSpeed * Time.deltaTime);
            // 👇 Dragoon만 후퇴 로직
            if (enemy is Dragoon dragoon)
            {
                float distance = Vector3.Distance(dragoon.transform.position, dragoon.targetPlayer.position);

                if (distance < dragoon.stopDistance - dragoon.bufferDistance)
                {
                    // 후퇴 중일 땐 SetDestination 비활성
                    dragoon.navMeshAgent.ResetPath();
                    Vector3 retreatDirection = -direction;
                    dragoon.navMeshAgent.Move(retreatDirection * dragoon.retreatSpeed * Time.deltaTime);
                    return;
                }
                else
                {
                    dragoon.navMeshAgent.SetDestination(dragoon.targetPlayer.position);
                }
            }
            else
            {
                enemy.navMeshAgent.SetDestination(enemy.targetPlayer.position);
            }
        }

        // 공격 범위 체크는 후퇴 안 하고 있을 때만
        float attackDistance = Vector3.Distance(enemy.transform.position, enemy.targetPlayer.position);
        if (attackDistance < enemy.attackRange)
        {
            enemy.ChangeState(new EnemyAttackState());
        }
    }

    public void FixedUPdateState(EnemyCtrl enemy)
    {
        
    }


    public void ExitState(EnemyCtrl enemy)
    {
        if(enemy.pv.isMine)
        {
            var agent = enemy.GetComponent<NavMeshAgent>();
            if (agent != null && agent.enabled && agent.isOnNavMesh)
            {
                agent.isStopped = true;
            }
        }
    }
}

