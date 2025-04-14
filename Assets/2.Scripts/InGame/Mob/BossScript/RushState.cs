using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RushState : BossState
{
    public float rushSpeed = 20f;
    private Vector3 rushDirection;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isRushing = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        base.OnStateEnter(animator, stateInfo, layerIndex);

        if (boss.currentTarget == null)
        {
            Debug.LogWarning("[RushState] currentTarget�� null�Դϴ�. ���� ��ȯ.");
            return;
        }

        animator.speed = 5f;
        Vector3 bossPos = animator.transform.position;
        Vector3 dir = (boss.currentTarget.transform.position - bossPos).normalized;
        dir.y = 0f;

        float rushDistance = GetScaledDetectionRadius(animator.transform) * 0.5f;
        rushDirection = dir;
        startPosition = bossPos;
        targetPosition = bossPos + dir * rushDistance;
        isRushing = true;

        Debug.Log($"[Rush] {boss.currentTarget.name} �������� ���� ����!");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isRushing) return;

        Transform bossTransform = animator.transform;
        bossTransform.position = Vector3.MoveTowards(
            bossTransform.position,
            targetPosition,
            rushSpeed * Time.deltaTime
        );

        if (Vector3.Distance(bossTransform.position, targetPosition) < 0.1f)
        {
            isRushing = false;
            animator.SetBool("Return", true);
            Debug.Log("[Rush] ���� �Ϸ�.");
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        isRushing = false;
        animator.speed = 1f;
    }
}
