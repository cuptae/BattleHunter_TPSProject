using UnityEngine;

public class Monster : MonoBehaviour
{
    [Header("HP 설정")]
    public float maxHP = 100f;
    public float currentHP;

    [Header("패트롤 설정")]
    public float patrolRange = 3f;     
    public float patrolSpeed = 2f;     

    private Vector3 initialPosition;
    private MonsterHPBar hpBar;

    private void Start()
{
    currentHP = maxHP;
    initialPosition = transform.position;
}



    private void Update()
    {
        // 🏃 몬스터 패트롤 이동
        float offset = Mathf.PingPong(Time.time * patrolSpeed, patrolRange * 2) - patrolRange;
        transform.position = initialPosition + new Vector3(offset, 0, 0);

        // 🎯 테스트용: 스페이스바로 공격
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10f);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);

        Debug.Log($"[TakeDamage] HP 감소: {currentHP} / {maxHP}");

        if (hpBar != null)
        {
            hpBar.UpdateHPBarUI();
        }
        
        if (currentHP <= 0)
        {
            Die();
        }
    }

    protected void Die()
    {
        Debug.Log($"{gameObject.name}이(가) 사망했습니다!");
        Destroy(gameObject);
    }
}
