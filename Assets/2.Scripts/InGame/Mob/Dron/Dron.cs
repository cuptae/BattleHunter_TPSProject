using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Dron : EnemyCtrl
{
    public float speed = 5f; // �̵� �ӵ�
    public float floatSpeed = 2f; // �����ִ� ���� ���� �̵��ϴ� �ӵ�
    public int damage = 10; // ���Ͱ� ���ϴ� ������
    public Collider upcollider; // ��Ȱ��ȭ�� �ݶ��̴�

    private Transform player;
    private Rigidbody rb;
    private bool isMoving = true; // �̵� ����
    private Coroutine movementCoroutine;

    private float playerSearchCooldown = 1f;
    private float lastPlayerSearchTime = 0f;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();

        // NavMeshAgent ��Ȱ��ȭ (�ʿ��� ���)
        var agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
        }

        // �ڷ�ƾ ����
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
        movementCoroutine = StartCoroutine(MovementRoutine());
    }

    void OnDisable()
    {
        // �ڷ�ƾ ����
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
            movementCoroutine = null;
        }
    }

    void Update()
    {
        if (Time.time - lastPlayerSearchTime >= playerSearchCooldown)
        {
            FindClosestPlayer();
            lastPlayerSearchTime = Time.time;
        }

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
            rb.velocity = Vector3.zero;
            MoveUpWhenStopped();           
        }
        if (upcollider != null)
        {
            upcollider.enabled = isMoving;
        }
    }

    void MoveTowardsPlayer()
    {
        if (player == null) return;

        Vector3 targetPosition = new Vector3(player.position.x, player.position.y + 1f, player.position.z);
        Vector3 direction = (targetPosition - transform.position).normalized;
        rb.velocity = direction * speed;
    }

    void MoveUpWhenStopped()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 10f))
        {
            float targetY = hit.point.y + 2f;

            // ���� ���̰� ��ǥ���� ���� ���� ���� �̵�
            if (transform.position.y < targetY)
            {
                // ���� �̵�
                Vector3 upwardMovement = Vector3.up * floatSpeed * Time.deltaTime;

                // �ڷδ� ������ �̵�
                Vector3 backwardMovement = -transform.forward * 1.0f * Time.deltaTime;

                // ����
                transform.position += upwardMovement + backwardMovement;
            }
            else
            {
                // ���δ� �� �ö�����, �ڷδ� ��� �а� �ʹٸ� �� ��Ͽ� ���Խ��ѵ� ��
                Vector3 backwardMovement = -transform.forward * 1.0f * Time.deltaTime;
                transform.position += backwardMovement;
            }
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
            yield return new WaitForSeconds(2f);
        }
    }

    // �浹 �� ������
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCtrl playerDamage = other.GetComponent<PlayerCtrl>();
            isMoving = false;
        
            if (playerDamage != null)
            {
                playerDamage.GetDamage(damage);
            }
        }
    }
}
