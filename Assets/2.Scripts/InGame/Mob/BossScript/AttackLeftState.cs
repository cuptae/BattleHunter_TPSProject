using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackLeftState : BossState
{
    private Collider collider1;
  

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex); // boss 참조 획득

        animator.speed = 2f;
        animator.SetBool("Return", true);

        if (boss.ColliderObject1 != null)
        {
            collider1 = boss.ColliderObject1.GetComponent<Collider>();
            if (collider1 != null) collider1.enabled = true;
        }


        Debug.Log("AttackStartState: 콜라이더 활성화 완료");
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 2f;
    }
}
