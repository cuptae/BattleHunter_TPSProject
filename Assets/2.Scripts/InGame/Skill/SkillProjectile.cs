using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SkillProjectile : MonoBehaviour
{
    ActiveData data;

    void OnCollisionEnter(Collision collision)
    {
        Explosion();
        PoolManager.Instance.ReturnObject(this.transform.name,this.gameObject);
    }

    public void Explosion()
    {
        if(data == null)
        {
            Collider[] enemyCols = Physics.OverlapSphere(transform.position,3,GameManager.Instance.enemyLayerMask);

            foreach(Collider col in enemyCols)
            {
                col.GetComponent<EnemyCtrl>().GetDamage(300);
            }
        }
        else
        {
            Collider[] enemyCols = Physics.OverlapSphere(transform.position,data.attackRange,GameManager.Instance.enemyLayerMask);

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
