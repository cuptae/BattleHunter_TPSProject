using UnityEngine;

public class MainHealth : MonoBehaviour
{
    [Header("�ڽ� ü�� �ڵ� ����")]
    public ChildHealth[] children;

    [Header("�� ü�� (�ڽ� ü���� �հ�)")]
    public int totalHealth;  // �ν����Ϳ��� ���̵��� �ʵ�� �ۼ�

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
        totalHealth = 0;
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
