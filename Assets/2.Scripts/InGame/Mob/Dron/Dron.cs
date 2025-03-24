using System.Collections;
using UnityEngine;

public class Dron : EnemyCtrl
{
    public float speed = 30f; // 이동 속도
    public float floatSpeed = 2f; // 멈춰있는 동안 위로 이동하는 속도
    public int damage = 10; // 몬스터가 가하는 데미지
    private Transform player;
    private Rigidbody rb;
    private bool isMoving = true; // 이동 여부

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(MovementRoutine());
    }

    void Update()
    {
        FindClosestPlayer(); // 가장 가까운 플레이어 찾기

        if (player != null)
        {
            LookAtPlayer(); // 플레이어 바라보기
        }

        if (isMoving && player != null)
        {
            MoveTowardsPlayer();
        }
        else
        {
            rb.velocity = Vector3.zero; // 멈춰있을 때 이동 정지
            MoveUpWhenStopped(); // Y 좌표를 3까지 올리기
        }
    }

    void MoveTowardsPlayer()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    void MoveUpWhenStopped()
    {
        if (transform.position.y < 3f) // Y 좌표가 3보다 작으면 위로 이동
        {
            transform.position += Vector3.up * floatSpeed * Time.deltaTime;
        }
    }

    void LookAtPlayer()
    {
        if (player == null) return;

        Vector3 lookDirection = (player.position - transform.position).normalized;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), Time.deltaTime * 5f);
    }

    void FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0)
        {
            player = null;
            return;
        }

        float closestDistance = Mathf.Infinity;
        Transform closestPlayer = null;

        foreach (GameObject p in players)
        {
            float distance = Vector3.Distance(transform.position, p.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPlayer = p.transform;
            }
        }

        player = closestPlayer;
    }

    IEnumerator MovementRoutine()
    {
        while (true)
        {
            isMoving = player != null;
            yield return new WaitForSeconds(0.15f);
            isMoving = false;
            yield return new WaitForSeconds(4f);
        }
    }

    // 충돌 감지 후 플레이어에게 데미지
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

    //        if (playerHealth != null)
    //        {
    //            playerHealth.TakeDamage(damage);
    //        }
    //    }
    //}
}
