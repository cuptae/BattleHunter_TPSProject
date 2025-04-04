using UnityEngine;

public class MainHealth : MonoBehaviour
{
    [Header("자식 체력 자동 수집")]
    public ChildHealth[] children;

    [Header("총 체력 (자식 체력의 합계)")]
    public int totalHealth;  // 인스펙터에서 보이도록 필드로 작성

    public int TotalHealth => totalHealth;  // 읽기 전용 프로퍼티

    void Awake()
    {
        // 자식 오브젝트에서 ChildHealth 컴포넌트 자동 수집
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
