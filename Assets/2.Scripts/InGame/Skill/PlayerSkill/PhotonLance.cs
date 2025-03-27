using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLance : ActiveSkill
{
    public PhotonLance(ActiveData activeData):base(activeData){}
    public override IEnumerator Activation()
    {
        Debug.Log("포톤랜스 발동");
        yield return null;
    }

}
