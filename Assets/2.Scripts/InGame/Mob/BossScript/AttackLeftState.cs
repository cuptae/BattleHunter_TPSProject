using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackLeftState : BossState
{
    private Collider collider1;
  

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex); // boss ���� ȹ��

        animator.speed = 2f;
        animator.SetBool("Return", true);

        if (boss.ColliderObject1 != null)
        {
            collider1 = boss.ColliderObject1.GetComponent<Collider>();
            if (collider1 != null) collider1.enabled = true;
        }


        Debug.Log("AttackStartState: �ݶ��̴� Ȱ��ȭ �Ϸ�");
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.speed = 2f;
    }
}
