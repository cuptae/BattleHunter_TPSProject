
using UnityEngine;
using UnityEngine.AI;

public class EnemyStunState : IEnemyState
{
    float time;
    float elapseTime;
    public EnemyStunState(float time)
    {
        this.time = time;
    }
    public void EnterState(EnemyCtrl enemy)
    { 
        enemy.currState = EnemyState.STUN;
        if(enemy.pv.isMine)
        {
            var agent = enemy.GetComponent<NavMeshAgent>();
            if (agent != null && agent.enabled && agent.isOnNavMesh)
            {
                Debug.Log("Stun State");
                agent.isStopped = true;
                agent.ResetPath(); // 필요 시
            }
        }
    }

    public void UpdateState(EnemyCtrl enemy)
    {
        elapseTime += Time.deltaTime;
        if(elapseTime>time)
        {
            if(enemy.hpBar.deBuff[1]!=null)
            {
                //enemy.hpBar.deBuff[1].SetActive(false);
                enemy.DisableDebuffMark(1);
            }
            enemy.ChangeState(new EnemyChaseState());
        }
    }

    public void FixedUPdateState(EnemyCtrl enemy)
    {

    }

    public void ExitState(EnemyCtrl enemy)
    {

    }
}
