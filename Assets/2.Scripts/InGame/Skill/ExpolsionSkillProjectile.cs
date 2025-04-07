using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ExpolsionSkillProjectile : MonoBehaviour
{
    ActiveData data;
    public GameObject effectVfx;

    void OnCollisionEnter(Collision collision)
    {
        Explosion();
        ContactPoint contact = collision.contacts[0];
        Vector3 hitPos = contact.point;
        Quaternion hitRot = Quaternion.LookRotation(contact.normal);
        GameObject fx = PoolManager.Instance.GetObject(data.skillName + "Vfx", hitPos, hitRot);
        PoolManager.Instance.ReturnObject(this.transform.name,this.gameObject);
    }

    public void Explosion()
    {
        Collider[] enemyCols = Physics.OverlapSphere(transform.position,data.attackRange,GameManager.Instance.enemyLayerMask);

        if(enemyCols != null)
        {
            foreach(Collider col in enemyCols)
            {
                col.GetComponent<EnemyCtrl>().GetDamage(data.damage);
            }
        }
    }

    public void SetProjectileData(ActiveData data)
    {  
        this.data = data;
    }

    public ActiveData GetProjecitleData()
    {   
        return data;
    }
}
