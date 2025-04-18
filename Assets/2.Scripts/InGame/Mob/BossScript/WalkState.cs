using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkState : BossState
{
    public float moveSpeed = 3f;
    private NavMeshAgent agent;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        animator.speed = 1f;
        animator.SetBool("Return", true); // 걷기 시작

        agent = animator.GetComponent<NavMeshAgent>();
        if (agent != null && boss.currentTarget != null)
        {
            agent.isStopped = false;
            agent.speed = moveSpeed;
            agent.SetDestination(boss.currentTarget.transform.position);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss.currentTarget != null && agent != null)
        {
            agent.SetDestination(boss.currentTarget.transform.position);

            float distance = agent.remainingDistance;

            if (distance > 7f) agent.speed = 3f;
            else agent.speed = 1f;

            if (distance > 3f)
            {
                // 자동 회전이 꺼져 있다면 수동 회전
                Vector3 direction = agent.steeringTarget - animator.transform.position;
                direction.y = 0f;

                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, targetRotation, Time.deltaTime * 5f);
                }
            }
            else
            {
                agent.isStopped = true;
                animator.SetTrigger("Tag");
                Debug.Log("WalkState: 타겟과의 거리가 3 이하입니다. 상태 전환 준비 완료.");
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 1f;
        if (agent != null)
        {
            agent.isStopped = true;
        }
    }
}

