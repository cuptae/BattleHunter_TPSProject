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
        gameObject.SetActive(false); // ������ �� ��Ȱ��ȭ
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

        // (����) ��ǥ���� �Ÿ��� �ʹ� �ָ� ��������
        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        // ���⼭ ������ ó�� ���� �� �� �� �־��
        isActive = false;
        gameObject.SetActive(false);

        // ���� �ð� �� ����ġ�� ����
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
