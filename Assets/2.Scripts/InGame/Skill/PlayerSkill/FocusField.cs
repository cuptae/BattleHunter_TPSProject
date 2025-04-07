using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusField : ActiveSkill
{
    public FocusField(ActiveData activeData,GameObject effectVfx,PlayerCtrl player):base(activeData,effectVfx,player){}

    public override IEnumerator Activation()
    {
        yield return null;
    }
}

