using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Dron : EnemyCtrl
{
    public float speed = 5f; // 이동 속도
    public float floatSpeed = 2f; // 멈춰있는 동안 위로 이동하는 속도
    public int damage = 10; // 몬스터가 가하는 데미지
    public Collider upcollider; // 비활성화할 콜라이더

    private Transform player;
    private Rigidbody rb;
    private bool isMoving = true; // 이동 여부
    private Coroutine movementCoroutine;

    private float playerSearchCooldown = 1f;
    private float lastPlayerSearchTime = 0f;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();

        // NavMeshAgent 비활성화 (필요한 경우)
        var agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
        }

        // 코루틴 시작
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
        movementCoroutine = StartCoroutine(MovementRoutine());
    }

    void OnDisable()
    {
        // 코루틴 정지
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
            LookAtPlayer(); // 플레이어 바라보기
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

            // 현재 높이가 목표보다 낮을 때만 위로 이동
            if (transform.position.y < targetY)
            {
                // 위로 이동
                Vector3 upwardMovement = Vector3.up * floatSpeed * Time.deltaTime;

                // 뒤로는 무조건 이동
                Vector3 backwardMovement = -transform.forward * 1.0f * Time.deltaTime;

                // 적용
                transform.position += upwardMovement + backwardMovement;
            }
            else
            {
                // 위로는 다 올라갔지만, 뒤로는 계속 밀고 싶다면 이 블록에 포함시켜도 됨
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

    // 충돌 시 데미지
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
