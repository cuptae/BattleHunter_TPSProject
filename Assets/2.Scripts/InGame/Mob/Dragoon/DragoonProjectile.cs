using UnityEngine;

public class DragoonProjectile : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 direction;
    private bool isActive = false;
    private Vector3 startPosition;
    public int damage = 10;

    void OnEnable()
    {
        Invoke("ResetProjectile",2.0f);
    }

    void Start()
    {
        startPosition = transform.position;
    }

    public void Launch(Vector3 shootDirection)
    {
        direction = shootDirection.normalized;
    }


    void Update()
    {
        //if (!isActive) return;

        transform.position += direction * speed * Time.deltaTime;
    }

    void HitTarget()
    {
        isActive = false;
        gameObject.SetActive(false);

        Invoke(nameof(ResetProjectile), 1.5f);
    }

    void ResetProjectile()
    {
        PoolManager.Instance.ReturnObject("DragoonProjectile",this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCtrl playerdamege = other.GetComponent<PlayerCtrl>();

            if (playerdamege != null)
            {
                playerdamege.GetDamage(damage);
                PoolManager.Instance.ReturnObject("DragoonProjectile",this.gameObject);
            }
        }
    }
}
