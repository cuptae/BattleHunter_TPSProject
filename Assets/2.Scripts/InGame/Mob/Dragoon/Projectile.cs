using UnityEngine;

public class DragoonProjectile : MonoBehaviour
{
    public float speed = 10f;
    private Transform target;
    private bool isActive = false;
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
        gameObject.SetActive(false); // 시작할 때 비활성화
    }

    public void Launch(Transform targetTransform)
    {
        target = targetTransform;
        isActive = true;
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (!isActive || target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // (선택) 목표와의 거리가 너무 멀면 꺼버리기
        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        // 여기서 데미지 처리 같은 걸 할 수 있어요
        isActive = false;
        gameObject.SetActive(false);

        // 일정 시간 후 원위치로 복귀
        Invoke(nameof(ResetProjectile), 1.5f);
    }

    void ResetProjectile()
    {
        transform.position = startPosition;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;
        if (other.CompareTag("Player"))
        {
            HitTarget();
        }
    }
}
