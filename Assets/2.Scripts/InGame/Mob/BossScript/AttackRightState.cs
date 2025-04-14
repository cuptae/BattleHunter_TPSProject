using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRightState : BossState
{
    private Collider collider1;
    private Collider collider2;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex); // boss 참조 획득

        animator.speed = 2f;
        animator.SetBool("Return", true);

      

        if (boss.ColliderObject2 != null)
        {
            collider2 = boss.ColliderObject2.GetComponent<Collider>();
            if (collider2 != null) collider2.enabled = true;
        }

        Debug.Log("AttackStartState: 콜라이더 활성화 완료");
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 1f;
    }
}
