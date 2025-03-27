using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class ActiveSkill : ISkill
{
    ActiveData activeData;
    public ActiveSkill(ActiveData activeData)
    {
        this.activeData = activeData;
    }
    public abstract IEnumerator Activation();
}
