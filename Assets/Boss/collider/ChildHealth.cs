using System.Collections.Generic;
using UnityEngine;

public class ChildHealth : MainHealth
{
    [Header("개별 체력 설정")]
    public int maxHealth = 100;

    [Header("현재 체력")]
    public int currentHealth;

    [Header("체력이 10% 이하일 때 비활성화할 오브젝트들")]
    public List<GameObject> objectsToDeactivate;

    private bool hasDeactivated = false;  // 중복 비활성화 방지

    private PhotonView pv;

    public int CurrentHealth => currentHealth;
    public int Hp => currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        pv = GetComponent<PhotonView>();
    }

    //  외부에서 데미지 요청할 함수 (Gunner 등에서 호출됨)
    public void GetDamage(int damage)
    {
        if (PhotonNetwork.isMasterClient)
        {
            pv.RPC("TakeDamageRPC", PhotonTargets.AllBuffered, damage);
        }
    }

    //  실제 데미지 처리 및 체력 감소 (모든 클라이언트에서 실행)
    [PunRPC]
    public void TakeDamageRPC(int damage)
    {
        currentHealth -= damage;

        Debug.Log($"[자식: {gameObject.name}] 데미지 {damage} ▶ 남은 체력: {currentHealth}");

        // 체력이 10% 이하일 때 오브젝트 비활성화
        if (!hasDeactivated && currentHealth <= maxHealth * 0.1f)
        {
            foreach (var obj in objectsToDeactivate)
            {
                if (obj != null && obj.activeSelf)
                {
                    obj.SetActive(false);
                    Debug.Log($"▶ {obj.name} 비활성화됨");
                }
            }
            hasDeactivated = true;
        }

        // 부모(MainHealth)에게 체력 갱신 알리기
        if (transform.parent.TryGetComponent<MainHealth>(out var parent))
        {
            parent.ForceUpdateHealth();
            Debug.Log($"[부모 총 체력] ▶ {parent.TotalHealth}");

            if (parent.TotalHealth <= 0)
            {
                Debug.Log("[부모] 체력 0 이하! 죽음!");
                // 죽음 처리 로직 호출 가능
            }
        }
    }

    public override int GetHealth()
    {
        return currentHealth;
    }

    protected override void Update() { }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Damage"))
        {
            // 데미지 트리거 예시 (사용 시 GetDamage 호출)
            // DamageDealer dealer = other.GetComponent<DamageDealer>();
            // if (dealer != null)
            // {
            //     GetDamage(dealer.damage);
            // }
        }
    }
}
