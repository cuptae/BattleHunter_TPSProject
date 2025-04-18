using UnityEngine;

public class MainHealth : MonoBehaviour
{
    [Header("�ڽ��� ü��")]
    public int mainHealth = 1000; // �ڱ� �ڽ��� ü��

    [Header("�ڽ� ü�� �ڵ� ����")]
    public ChildHealth[] children;

    [Header("�� ü�� (�ڽ� + �ڽ� ü���� �հ�)")]
    public int totalHealth;  // �ν����Ϳ� ���̵��� �ʵ�� ����

    public int TotalHealth => totalHealth;  // �б� ���� ������Ƽ

    void Awake()
    {
        // �ڽ� ������Ʈ���� ChildHealth ������Ʈ �ڵ� ����
        children = GetComponentsInChildren<ChildHealth>();
    }

    protected virtual void Update()
    {
        UpdateTotalHealth();
    }

    protected void UpdateTotalHealth()
    {
        totalHealth = mainHealth; // �ڽ��� ü���� ���� ����
        foreach (var child in children)
        {
            totalHealth += child.GetHealth();
        }
    }

    public virtual int GetHealth()
    {
        return TotalHealth;
    }

    public void ForceUpdateHealth()
    {
        UpdateTotalHealth();
    }
}
