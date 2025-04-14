using UnityEngine;

public class PatternState : BossState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        // "Return" �Ķ���Ͱ� false�� ���� ����
        if (!animator.GetBool("Return"))
        {
            int patternIndex = Random.Range(0, 0); // 0���� 4������ ���� �� ���� ����

            switch (patternIndex)
            {
                case 0:
                    animator.SetTrigger("Rush");
                    Debug.Log("����");
                    break;
                //case 1:
                //    animator.SetTrigger("LeftAttack");
                //    Debug.Log("������");
                //    break;
                //case 2:
                //    animator.SetTrigger("RightAttack");
                //    Debug.Log("��");
                //    break;
                //case 3:
                //    animator.SetTrigger("Crush");
                //    Debug.Log("��");
                //    break;
                //case 4:
                //    animator.SetTrigger("Missile");
                //    Debug.Log("����");
                //    break;
            }
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // ������ ���� ���� �� ���� ������ �ʿ��ϸ� ���⿡ �ۼ�
    }
}
