using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Tool Class", menuName = "Item/Tool")]
public class ToolClass : ItemClass //도구 아이템(무기 같은거)의 정의에 대한 클래스
{
    [Header("Tool")]
    public ToolType toolType;
    public bool isEquip = false;
    public float damage;
    
    public enum ToolType
    {
        Bow,
        Hammer,
        Axe,
        Sword
    }

    public override void Use(PlayerCtrl caller)
    {
        base.Use(caller);
        Debug.Log("Swing Tool");
        //caller.HandleWeapon
        //caller.MatchingWeapon(this);
    }
    public override ToolClass GetTool() { return this; }
   
}
