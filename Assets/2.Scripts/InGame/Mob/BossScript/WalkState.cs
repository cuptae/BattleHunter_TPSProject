using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : BossState
{
    public float moveSpeed = 3f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        animator.speed = 1f;
        animator.SetBool("Return", true); // 걷기 시작
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss.currentTarget != null)
        {
            Vector3 targetPosition = boss.currentTarget.transform.position;
            Vector3 direction = targetPosition - animator.transform.position;
            direction.y = 0f;

            float distance = direction.magnitude;

            if (distance>7f){ moveSpeed = 3f;}
            else{ moveSpeed = 1f; }

            if (distance > 3f)
            {
                // 이동 및 회전 (선택사항)
                Vector3 moveDirection = direction.normalized;
                animator.transform.position += moveDirection * moveSpeed * Time.deltaTime;

                // 타겟 방향을 바라보도록 회전
                if (moveDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                    animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, targetRotation, Time.deltaTime * 5f);
                }
            }
            else
            {
                animator.SetTrigger("Tag");
                Debug.Log("WalkState: 타겟과의 거리가 3 이하입니다. 상태 전환 준비 완료.");
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 1f;
    }
}

