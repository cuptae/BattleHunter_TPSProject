using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Tool Class", menuName = "Item/Misc")]
public class MiscClass : ItemClass //기타 아이템 (재료나 퀘스트템 같은)
{

    public override void Use(PlayerCtrl caller)
    {
        base.Use(caller);
    }
    public override MiscClass GetMisc() { return null; }
}
