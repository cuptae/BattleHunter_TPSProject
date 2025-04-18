using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ExpolsionSkillProjectile : MonoBehaviour
{
    ActiveData data;
    public GameObject effectVfx;


    void OnTriggerEnter(Collider other)
    {
        Vector3 hitPos = transform.position; // 또는 other.ClosestPoint(transform.position);
        Vector3 direction = (other.transform.position - transform.position).normalized;
        Quaternion hitRot = Quaternion.LookRotation(direction);

        PoolManager.Instance.GetObject(data.skillName +"Vfx", hitPos, hitRot);
        Explosion();
    }

    public void Explosion()
    {
        Collider[] enemyCols = Physics.OverlapSphere(transform.position,data.attackRange,GameManager.Instance.enemyLayerMask);

        if(enemyCols != null)
        {
            foreach(Collider col in enemyCols)
            {
                col.GetComponent<IDamageable>().GetDamage(data.damage,null);
            }
        }
        PoolManager.Instance.ReturnObject(this.transform.name,this.gameObject);
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
