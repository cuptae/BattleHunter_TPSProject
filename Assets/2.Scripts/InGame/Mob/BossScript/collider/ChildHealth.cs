using System.Collections.Generic;
using UnityEngine;

public class ChildHealth : MainHealth,IDamageable
{
    public int maxHealth = 100;

    public int currentHealth;

    public List<GameObject> objectsToDeactivate;

    private bool hasDeactivated = false;

    private PhotonView pv;

    public int CurrentHealth => currentHealth;
    public int Hp => currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        pv = GetComponent<PhotonView>();
    }

    public void GetDamage(int damage)
    {
        if (PhotonNetwork.isMasterClient)
        {
            pv.RPC("TakeDamageRPC", PhotonTargets.AllBuffered, damage);
        }
    }

    [PunRPC]
    public void TakeDamageRPC(int damage)
    {
        currentHealth -= damage;

        if (!hasDeactivated && currentHealth <= maxHealth * 0.1f)
        {
            foreach (var obj in objectsToDeactivate)
            {
                if (obj != null && obj.activeSelf)
                {
                    obj.SetActive(false);
                }
            }
            hasDeactivated = true;
        }

        if (transform.parent.TryGetComponent<MainHealth>(out var parent))
        {
            parent.ForceUpdateHealth();
        }
    }

    public override int GetHealth()
    {
        return currentHealth;
    }

    protected override void Update() { }

}
