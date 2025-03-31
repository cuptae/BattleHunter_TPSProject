using UnityEngine;

public class HealPack : MonoBehaviour
{
    public GameObject Heal;
    public int damage = -30;

    void Start()
    {
        Heal.SetActive(true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCtrl playerdamege = other.GetComponent<PlayerCtrl>();

            if (playerdamege != null)
            {
                playerdamege.GetDamage(damage);
            }
           Heal.SetActive(false);
        }
    }
}
