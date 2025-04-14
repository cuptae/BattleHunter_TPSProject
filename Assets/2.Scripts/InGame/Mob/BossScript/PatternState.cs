using UnityEngine;

public class PatternState : BossState
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        // "Return" 파라미터가 false일 때만 실행
        if (!animator.GetBool("Return"))
        {
            int patternIndex = Random.Range(0, 0); // 0부터 4까지의 정수 중 랜덤 선택

            switch (patternIndex)
            {
                case 0:
                    animator.SetTrigger("Rush");
                    Debug.Log("돌진");
                    break;
                //case 1:
                //    animator.SetTrigger("LeftAttack");
                //    Debug.Log("오른녹");
                //    break;
                //case 2:
                //    animator.SetTrigger("RightAttack");
                //    Debug.Log("앞");
                //    break;
                //case 3:
                //    animator.SetTrigger("Crush");
                //    Debug.Log("뒤");
                //    break;
                //case 4:
                //    animator.SetTrigger("Missile");
                //    Debug.Log("위쪽");
                //    break;
            }
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 패턴이 실행 중일 때 별도 로직이 필요하면 여기에 작성
    }
}
