using UnityEngine;
using UnityEngine.AI;

public class DragoonPre : EnemyCtrl
{
    public float stopDistance = 10f;
    public float retreatSpeed = 3f;
    public float bufferDistance = 1f;
    public float rotationSpeed = 5f;

    public float attackRange = 11f; // ���� ��Ÿ�
    public float fireInterval = 3f; // �߻� ����

    public GameObject firePoint; // ����ü ���� ��ġ

    private NavMeshAgent agent;
    private Transform targetPlayer;
    private float fireTimer;
    public DragoonProjectile projectile; // �������� �ƴ�, ���� ��ġ�� ���� ����ü

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

        // ȸ��
        Vector3 direction = (targetPlayer.position - transform.position).normalized;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        // �̵�
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

        // ����
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
            fireTimer = fireInterval; // ��Ÿ� ����� Ÿ�̸� ����
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

        //�ݵ�: ��� �ڷ� 0.5f �̵�
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
