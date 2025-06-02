using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotClass
{
    [SerializeField] public ItemClass item; // 슬롯 내에 아이템 정보
    [SerializeField] public int count; //아이템의 개수

    public SlotClass() //빈슬롯
    {
        item = null;
        count = 0;
    }

    public SlotClass(ItemClass _item, int _count) //아이템과 수량 슬롯
    {
        item = _item;
        count = _count;
    }

    public SlotClass(SlotClass slot) //기존 슬롯을 복사해 새로운 슬롯 생성
    {
        this.item = slot.GetItem();
        this.count = slot.GetCount();
    }

    public void Clear() //슬롯 초기화
    {
        this.item = null;
        this.count = 0;
    }

    public ItemClass GetItem() { return item; } //현재 슬롯 안에 아이템 리턴
    public int GetCount() { return count; } // 현재 슬롯 안에 수량 리턴
    public void AddCount(int _count) { count += _count; } //현재 수량에 더함 (겹쳐서 같은 거 없애는 기능)
    public void SubCount(int _count) //수량 감소
    {
        count -= _count;
        if (count <= 0)
        {
            Clear(); //줄이다가 개수가 0이하가 되면 슬롯을 초기화 시킴
        }
    }
    public void AddItem(ItemClass item, int count) //슬롯에 새 아이템과 수량을 설정시킴
    {
        this.item = item;
        this.count = count;
    }
    
    public void SetSlot(SlotClass slot)
    {
        this.item = slot.GetItem();
        this.count = slot.GetCount();
    }
}
