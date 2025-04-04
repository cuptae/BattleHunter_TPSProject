using System.Collections.Generic;
using UnityEngine;

public class ChildHealth : MainHealth
{
    [Header("���� ü�� ����")]
    public int maxHealth = 100;

    [Header("���� ü��")]
    public int currentHealth;

    [Header("ü���� 10% ������ �� ��Ȱ��ȭ�� ������Ʈ��")]
    public List<GameObject> objectsToDeactivate;

    private bool hasDeactivated = false;  // �ߺ� ��Ȱ��ȭ ����

    private PhotonView pv;

    public int CurrentHealth => currentHealth;
    public int Hp => currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        pv = GetComponent<PhotonView>();
    }

    //  �ܺο��� ������ ��û�� �Լ� (Gunner ��� ȣ���)
    public void GetDamage(int damage)
    {
        if (PhotonNetwork.isMasterClient)
        {
            pv.RPC("TakeDamageRPC", PhotonTargets.AllBuffered, damage);
        }
    }

    //  ���� ������ ó�� �� ü�� ���� (��� Ŭ���̾�Ʈ���� ����)
    [PunRPC]
    public void TakeDamageRPC(int damage)
    {
        currentHealth -= damage;

        Debug.Log($"[�ڽ�: {gameObject.name}] ������ {damage} �� ���� ü��: {currentHealth}");

        // ü���� 10% ������ �� ������Ʈ ��Ȱ��ȭ
        if (!hasDeactivated && currentHealth <= maxHealth * 0.1f)
        {
            foreach (var obj in objectsToDeactivate)
            {
                if (obj != null && obj.activeSelf)
                {
                    obj.SetActive(false);
                    Debug.Log($"�� {obj.name} ��Ȱ��ȭ��");
                }
            }
            hasDeactivated = true;
        }

        // �θ�(MainHealth)���� ü�� ���� �˸���
        if (transform.parent.TryGetComponent<MainHealth>(out var parent))
        {
            parent.ForceUpdateHealth();
            Debug.Log($"[�θ� �� ü��] �� {parent.TotalHealth}");

            if (parent.TotalHealth <= 0)
            {
                Debug.Log("[�θ�] ü�� 0 ����! ����!");
                // ���� ó�� ���� ȣ�� ����
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
            // ������ Ʈ���� ���� (��� �� GetDamage ȣ��)
            // DamageDealer dealer = other.GetComponent<DamageDealer>();
            // if (dealer != null)
            // {
            //     GetDamage(dealer.damage);
            // }
        }
    }
}
