using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : ActiveSkill
{
    public ShockWave(ActiveData activeData):base(activeData){}
    public override IEnumerator Activation()
    {
        Debug.Log("ShockWave Activate");
        yield return null;
    }

}
