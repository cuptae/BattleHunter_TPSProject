using UnityEngine;
using UnityEngine.AI;

public class Dragoon : MonoBehaviour
{
    public float stopDistance = 5f; // �ִ� ���� �Ÿ�
    public float retreatSpeed = 3f; // ���� �ӵ�
    public float bufferDistance = 1f; // ���� �����ϴ� ���� �Ÿ�
    public float rotationSpeed = 5f; // ȸ�� �ӵ�

    private NavMeshAgent agent;
    private Transform targetPlayer; // ���� ����� �÷��̾�

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        targetPlayer = FindClosestPlayer();

        if (targetPlayer == null) return;

        float distance = Vector3.Distance(transform.position, targetPlayer.position);

        // �׻� �÷��̾ �ٶ󺸰� ȸ�� (�ε巯�� ȸ�� ����)
        Vector3 direction = (targetPlayer.position - transform.position).normalized;
        direction.y = 0; // Y�� ȸ�� ���� (�ٴڸ� ���� �ʰ�)
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        if (distance > stopDistance)
        {
            // �÷��̾�� �ٰ�����
            agent.SetDestination(targetPlayer.position);
        }
        else if (distance < stopDistance - bufferDistance) // �ʹ� ������ ����
        {
            // ���� ����: ���� -> �÷��̾� ������ �ݴ�
            Vector3 retreatDirection = -direction;
            agent.Move(retreatDirection * retreatSpeed * Time.deltaTime);
        }
        else
        {
            agent.ResetPath(); // ������ �Ÿ����� ����
        }
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
