using UnityEngine;
using UnityEngine.AI;

public class Dragoon : EnemyCtrl
{
    public float stopDistance = 10f;
    public float retreatSpeed = 3f;
    public float bufferDistance = 1f;
    public float rotationSpeed = 5f;

    public float attackRange = 11f; // 공격 사거리
    public float fireInterval = 3f; // 발사 간격

    public GameObject firePoint; // 투사체 생성 위치

    private NavMeshAgent agent;
    private Transform targetPlayer;
    private float fireTimer;
    public DragoonProjectile projectile; // 프리팹이 아닌, 씬에 배치된 단일 투사체

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        fireTimer = fireInterval;
    }

    void Update()
    {
        targetPlayer = FindClosestPlayer();

        if (targetPlayer == null) return;

        float distance = Vector3.Distance(transform.position, targetPlayer.position);

        // 회전
        Vector3 direction = (targetPlayer.position - transform.position).normalized;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        // 이동
        if (distance > stopDistance)
        {
            agent.SetDestination(targetPlayer.position);
        }
        else if (distance < stopDistance - bufferDistance)
        {
            Vector3 retreatDirection = -direction;
            agent.Move(retreatDirection * retreatSpeed * Time.deltaTime);
        }
        else
        {
            agent.ResetPath();
        }

        // 공격
        if (distance <= attackRange)
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                FireProjectile();
                fireTimer = fireInterval;
            }
        }
        else
        {
            fireTimer = fireInterval; // 사거리 벗어나면 타이머 리셋
        }
    }


    void FireProjectile()
    {
        if (projectile != null && targetPlayer != null)
        {
            Vector3 shootDirection = (targetPlayer.position - firePoint.transform.position).normalized;
            projectile.transform.position = firePoint.transform.position;
            projectile.Launch(shootDirection);
        }

        // 반동: 드라군 뒤로 0.5f 이동
        Vector3 backDirection = -transform.forward;
        transform.position += backDirection * 0.5f;
    }

    Transform FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        Transform closestPlayer = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = player.transform;
            }
        }

        return closestPlayer;
    }
}
