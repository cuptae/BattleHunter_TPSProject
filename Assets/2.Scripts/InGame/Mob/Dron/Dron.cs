using System.Collections;
using UnityEngine;

public class Dron : EnemyCtrl
{
    public float speed = 30f; // �̵� �ӵ�
    public float floatSpeed = 2f; // �����ִ� ���� ���� �̵��ϴ� �ӵ�
    public int damage = 10; // ���Ͱ� ���ϴ� ������
    private Transform player;
    private Rigidbody rb;
    private bool isMoving = true; // �̵� ����

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(MovementRoutine());
    }

    void Update()
    {
        FindClosestPlayer(); // ���� ����� �÷��̾� ã��

        if (player != null)
        {
            LookAtPlayer(); // �÷��̾� �ٶ󺸱�
        }

        if (isMoving && player != null)
        {
            MoveTowardsPlayer();
        }
        else
        {
            rb.velocity = Vector3.zero; // �������� �� �̵� ����
            MoveUpWhenStopped(); // Y ��ǥ�� 3���� �ø���
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
        if (transform.position.y < 3f) // Y ��ǥ�� 3���� ������ ���� �̵�
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

    // �浹 ���� �� �÷��̾�� ������
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
