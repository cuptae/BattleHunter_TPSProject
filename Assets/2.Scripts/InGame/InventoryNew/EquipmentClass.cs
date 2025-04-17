using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Tool Class", menuName = "Item/Equipment")]
public class EquipmentClass : ItemClass //장비 아이템 클래스
{
    [Header("Equipment")]
    public float defenvalue;
    public bool isEquip;
    public ClothType clothType;
    public enum ClothType
    {
        Chest,
        Leg
    }
    public override void Use(PlayerCtrl caller)
    {
        base.Use(caller);
        Debug.Log("Equip!!");
        //caller.MatchingCloth(this);
        //caller.inventory.UseSelected(); // 플레이어 컨트롤러 스크립트에 Inventory inventory를 선언해줘야 사용가능
    }
    public override EquipmentClass GetEquipment() { return this; }
}
