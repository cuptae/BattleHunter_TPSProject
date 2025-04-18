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
        animator.SetBool("Return", true); // �ȱ� ����
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
                // �̵� �� ȸ�� (���û���)
                Vector3 moveDirection = direction.normalized;
                animator.transform.position += moveDirection * moveSpeed * Time.deltaTime;

                // Ÿ�� ������ �ٶ󺸵��� ȸ��
                if (moveDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                    animator.transform.rotation = Quaternion.Slerp(animator.transform.rotation, targetRotation, Time.deltaTime * 5f);
                }
            }
            else
            {
                animator.SetTrigger("Tag");
                Debug.Log("WalkState: Ÿ�ٰ��� �Ÿ��� 3 �����Դϴ�. ���� ��ȯ �غ� �Ϸ�.");
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 1f;
    }
}

