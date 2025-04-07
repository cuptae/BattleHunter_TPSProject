using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBurst : ActiveSkill
{
    public EnergyBurst(ActiveData activeData,GameObject effectVfx,PlayerCtrl player):base(activeData,effectVfx,player){}

    public override IEnumerator Activation()
    {
        yield return null;
    }
}
