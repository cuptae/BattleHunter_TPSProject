using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "new Tool Class", menuName = "Item/Consumable")]
public class ConsumableClass : ItemClass //아이템 효과와 데이터 저장
{
    [Header("Consumable")]
    public float healthAdded;
    public float staminaAdded;
    public override void Use(PlayerCtrl caller)
    {
        base.Use(caller);
        Debug.Log("Eat Consumable");
        //caller.EatFood(this.healthAdded, this.staminaAdded, this.itemIcon);
        //caller.inventory.UseSelected(); // 플레이어 컨트롤러 스크립트에 Inventory inventory를 선언해줘야 사용가능
    }
    public override ConsumableClass GetConsumable() { return this; }
}
