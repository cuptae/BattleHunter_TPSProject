using UnityEngine;

public class DragoonProjectile : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 direction;
    private bool isActive = false;
    private Vector3 startPosition;
    public int damage = 10;

    void Start()
    {
        startPosition = transform.position;
        gameObject.SetActive(false); // ������ �� ��Ȱ��ȭ
    }

    // ������ �����ؼ� �߻�
    public void Launch(Vector3 shootDirection)
    {
        direction = shootDirection.normalized;
        isActive = true;
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (!isActive) return;

        transform.position += direction * speed * Time.deltaTime;
    }

    void HitTarget()
    {
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
        if (other.CompareTag("Player"))
        {
            PlayerCtrl playerdamege = other.GetComponent<PlayerCtrl>();

            if (playerdamege != null)
            {
                playerdamege.GetDamage(damage);
            }
            HitTarget();
        }
    }
}
