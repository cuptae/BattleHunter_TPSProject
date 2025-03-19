using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeState : PlayerState
{
    public DodgeState(PlayerCtrl player) : base (player){}

    public override void EnterState()
    {
        player.StartCoroutine(DodgeRoutine());
    }

    public override void UpdateState() {}

    public override void ExitState() {}
    public override void FixedUpdateState()
    {
        
    }
    private IEnumerator DodgeRoutine()
    {
        player.animator.SetTrigger("Dodge");
        player.isDodge = true;
        yield return new WaitForSeconds(player.dodgeTime);
        player.isDodge = false;
        player.ChangeState(new IdleState(player));
    }

    
    // IEnumerator Dodge()
    // {
    //     if (isDodge)
    //         yield break;

    //     float elapseTime = 0f;
    //     dodgeDir = isMove ? moveDir : transform.forward;
    //     if (dodgeDir == Vector3.zero)
    //     {
    //         dodgeDir = transform.forward;  // 기본 방향 설정
    //     }
    //     Quaternion dodgeLook = isMove ? Quaternion.LookRotation(moveDir) : Quaternion.LookRotation(transform.forward);
    //     animator.SetTrigger("Dodge");

    //     // 회피 시 주변 몬스터와의 충돌을 무시
    //     Collider[] monCols = Physics.OverlapSphere(tr.position, 7.0f, enemyLayerMask);
    //     foreach (Collider monsterCol in monCols)
    //     {
    //         Physics.IgnoreCollision(col, monsterCol, true); // 적과의 충돌을 무시
    //     }

    //     while (elapseTime < dodgeTime)
    //     {
    //         isDodge = true;
    //         transform.rotation = dodgeLook;
    //         elapseTime += Time.deltaTime;
    //         yield return null;
    //     }

    //     isDodge = false;

    //     // 회피 종료 후 적과의 충돌을 다시 활성화
    //     foreach (Collider monsterCollider in monCols)
    //     {
    //         Physics.IgnoreCollision(col, monsterCollider, false); // 충돌을 다시 활성화
    //     }
    // }
}
