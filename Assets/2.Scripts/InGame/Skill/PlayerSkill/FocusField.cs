using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusField : ActiveSkill
{
    public FocusField(ActiveData activeData,GameObject effectVfx,PlayerCtrl player):base(activeData,effectVfx,player){}

    public override IEnumerator Activation()
    {
        if(ScanEnemySphere() != null)
        {
            foreach(EnemyCtrl enemy in ScanEnemySphere())
            {
                Debug.Log(enemy.transform.name);
                enemy.GetDamage(activeData.damage,activeData);
            }
        }
        yield return new WaitForSeconds(0.5f);
        Debug.Log("FocusField Finished");
        onSkillEnd?.Invoke();
    }
}

