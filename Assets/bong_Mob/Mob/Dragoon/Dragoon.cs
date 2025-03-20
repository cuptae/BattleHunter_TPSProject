using UnityEngine;
using UnityEngine.AI;

public class Dragoon : MonoBehaviour
{
    public float stopDistance = 5f; // 최대 접근 거리
    public float retreatSpeed = 3f; // 후퇴 속도
    public float bufferDistance = 1f; // 후퇴를 시작하는 여유 거리
    public float rotationSpeed = 5f; // 회전 속도

    private NavMeshAgent agent;
    private Transform targetPlayer; // 가장 가까운 플레이어

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        targetPlayer = FindClosestPlayer();

        if (targetPlayer == null) return;

        float distance = Vector3.Distance(transform.position, targetPlayer.position);

        // 항상 플레이어를 바라보게 회전 (부드러운 회전 적용)
        Vector3 direction = (targetPlayer.position - transform.position).normalized;
        direction.y = 0; // Y축 회전 제거 (바닥만 보지 않게)
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);

        if (distance > stopDistance)
        {
            // 플레이어에게 다가가기
            agent.SetDestination(targetPlayer.position);
        }
        else if (distance < stopDistance - bufferDistance) // 너무 가까우면 후퇴
        {
            // 후퇴 방향: 몬스터 -> 플레이어 방향의 반대
            Vector3 retreatDirection = -direction;
            agent.Move(retreatDirection * retreatSpeed * Time.deltaTime);
        }
        else
        {
            agent.ResetPath(); // 적절한 거리에서 정지
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
