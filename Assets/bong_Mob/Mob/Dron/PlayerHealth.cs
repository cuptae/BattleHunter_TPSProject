using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //�̽�ũ��Ʈ�� ���� �ʿ�� ������ ����� �����Ҷ� �÷��̾ �������ؼ� ���� ��ũ��Ʈ
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("�÷��̾ �������� ����! ���� ü��: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("�÷��̾� ���!");
        // ��� ó�� (��: ���� ���� ȭ��, ������ ��)
    }
}
