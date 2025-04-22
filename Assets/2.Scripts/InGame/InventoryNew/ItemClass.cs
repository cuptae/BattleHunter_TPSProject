using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ItemClass : ScriptableObject
{
    [Header("Item")]
    public int itemID; // 아이템 고유 번호 (제품번호)
    public string itemName; // 아이템 이름 (제품명)
    public string itemDesc; // 설명 텍스트 (힐템의 경우 HP +10 같은 느낌)
    public Sprite itemIcon; // 아이템 이미지 (UI에 표시됨)
    public bool isStackable = true; // 여러 개 쌓을 수 있는 아이템인지 여부 (일반적으로 소모템에 해당됨)

    public virtual void Use(PlayerCtrl caller) // 아이템 사용을 위해 호출에 사용되는 메서드
    {
        Debug.Log("Used Item : "); // 디버깅
    }
    public virtual ItemClass GetItem() { return this; } // 항상 자신을 반환함
    public virtual ToolClass GetTool() { return null; } // 아이템이 ToolClass 일 때 오버라이드 후 반환함
    public virtual MiscClass GetMisc() { return null; } // 기타 아이템 전용
    public virtual ConsumableClass GetConsumable() { return null; } // 소비 아이템 전용
    public virtual EquipmentClass GetEquipment() { return null; } // 장비 아이템 전용

}