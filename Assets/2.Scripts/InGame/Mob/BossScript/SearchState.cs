using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchState : BossState
{
    public float rotationSpeed = 0.5f;
    private bool isRotating = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        GameObject foundTarget = FindRandomTargetInRange(animator.transform);
        if (foundTarget != null)
        {
            Debug.Log($"[Search] 탐지된 플레이어 수: {playersInRange.Count}, 선택된 타겟: {foundTarget.name}");
            isRotating = true;
        }
        else
        {
            Debug.Log("[Search] 탐지 반경 내에 플레이어 없음.");
            animator.SetBool("Return", true);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (boss.currentTarget != null && isRotating)
        {
            Vector3 direction = (boss.currentTarget.transform.position - animator.transform.position).normalized;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                animator.transform.rotation = Quaternion.Slerp(
                    animator.transform.rotation,
                    targetRotation,
                    Time.deltaTime * rotationSpeed
                );

                float angle = Quaternion.Angle(animator.transform.rotation, targetRotation);
                if (angle < 1f)
                {
                    animator.transform.rotation = targetRotation;
                    isRotating = false;
                    animator.SetBool("Return", false);
                    Debug.Log("회전 완료. Return false 설정됨");
                }
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 1f;
    }
}
