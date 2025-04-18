using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //이스크립트는 딱히 필요는 없으나 드론이 공격할때 플레이어를 만들어야해서 만든 스크립트
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("플레이어가 데미지를 입음! 남은 체력: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("플레이어 사망!");
        // 사망 처리 (예: 게임 오버 화면, 리스폰 등)
    }
}
