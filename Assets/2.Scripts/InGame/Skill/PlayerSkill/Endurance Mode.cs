using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnduranceMode : ActiveSkill
{
    public EnduranceMode(ActiveData activeData,GameObject effectVfx,PlayerCtrl player):base(activeData,effectVfx,player){}

    public override IEnumerator Activation()
    {
        yield return null;
    }
}
