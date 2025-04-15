using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;

public class Dragoon : EnemyCtrl
{
    public float stopDistance = 10f;
    public float retreatSpeed = 3f;
    public float bufferDistance = 1f;

    public Transform firePos;


    protected override void Update()
    {
        base.Update();
        if(pv.isMine)
        {
            // //플레이어 접근하면 뒤로가는 코드
            // Vector3 direction = (targetPlayer.position - transform.position).normalized;
            // float distance = Vector3.Distance(transform.position, targetPlayer.position);
            // // if (distance > stopDistance)
            // // {
            // //     navMeshAgent.SetDestination(targetPlayer.position);
            // // }
            // if (distance < stopDistance - bufferDistance)
            // {
            //     Vector3 retreatDirection = -direction;
            //     navMeshAgent.Move(retreatDirection * retreatSpeed * Time.deltaTime);
            // }
            // else
            // {
            //     navMeshAgent.ResetPath();
            // }
        }
    }

    public override void Attack()
    {
        GameObject go = PoolManager.Instance.GetObject("DragoonProjectile",firePos.position,Quaternion.identity);
        DragoonProjectile projectile = go.GetComponent<DragoonProjectile>();
        if (projectile != null && targetPlayer != null)
        {
            Vector3 shootDirection = (targetPlayer.position - firePos.transform.position).normalized;
            projectile.transform.position = firePos.transform.position;
            projectile.Launch(shootDirection);
        }
        Vector3 backDirection = -transform.forward;
        transform.position += backDirection * 0.5f;
    }
}
