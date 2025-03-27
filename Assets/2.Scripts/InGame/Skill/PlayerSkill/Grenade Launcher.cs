using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : ActiveSkill
{
    public GrenadeLauncher(ActiveData activeData):base(activeData){}
    public override IEnumerator Activation()
    {
        Debug.Log("그레네이드 런처 Activate");
        yield return null;
    }

}
