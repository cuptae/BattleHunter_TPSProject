using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : IEnemyState
{
    public void EnterState(EnemyCtrl enemy)
    {
        enemy.isDead = true;
        enemy.navMeshAgent.isStopped = true;
        enemy.gameObject.SetActive(false); // 적 삭제
    }

    public void UpdateState(EnemyCtrl enemy)
    {
        // 사망 상태에서는 아무것도 안 함
    }

    public void ExitState(EnemyCtrl enemy)
    {
        // 사망 상태 해제 불가능
    }
}

